# OAuth2 인증 API 활용 커스텀 커넥터 만들기 #

파워 플랫폼 커스텀 커넥터에 연동시키기 위한 API를 개발합니다. 이 API는 애저 API 관리자에서 프록시 형태로 지원하는 OAuth2 인증을 구현합니다. 아래 순서대로 따라해 보세요.

- [OAuth2 인증 API 활용 커스텀 커넥터 만들기](#oauth2-인증-api-활용-커스텀-커넥터-만들기)
  - [1. OAuth2 인증용 앱 등록하기](#1-oauth2-인증용-앱-등록하기)
  - [2. API 앱 개발하기](#2-api-앱-개발하기)
  - [3. GitHub 액션 연동후 자동 배포하기](#3-github-액션-연동후-자동-배포하기)
  - [4. API 관리자 연동하기](#4-api-관리자-연동하기)
  - [5. 파워 플랫폼 커스텀 커넥터 생성하기](#5-파워-플랫폼-커스텀-커넥터-생성하기)
  - [6. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기](#6-파워-앱과-파워-오토메이트에서-커스텀-커넥터-사용하기)
    - [파워 오토메이트](#파워-오토메이트)
    - [파워 앱](#파워-앱)

## 1. OAuth2 인증용 앱 등록하기 ##

이번에 사용하려는 API는 OAuth2 인증을 통한 액세스 토큰이 필요합니다. 이 액세스 토큰을 발급받기 위해서는 먼저 [OAuth2 인증용 앱][az ad oauth2]을 [애저 액티브 디렉토리][az ad]에 등록해야 합니다. 아래 순서대로 따라해 보세요.

1. 애저 포털 상단의 검색창에 `azure active directory`를 검색합니다.

    ![애저 포털에서 액티브 디렉토리 검색하기][image01]

2. 액티브 디렉토리 화면에서 **앱 등록** 블레이드로 이동합니다. 그러면 이미 [애저 Dev CLI 이용해서 애저 인스턴스 만들기](./1-azd.md) 세션에서 애저 CLI를 통해 **spn-gppb{{랜덤숫자}}**라는 이름으로 만들었던 앱이 하나 있는 것이 보입니다. 여기서 `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ![등록한 앱 확인][image02]

3. 위의 앱은 GitHub 액션 워크플로우 용도로 만든 것이므로 사용하지 않고, API 관리자 용도로 하나 다시 만들겠습니다. **➕ 새 등록** 메뉴를 클릭합니다.

    ![새 앱 등록][image03]

4. 애플리케이션 등록 화면에서 아래와 같이 각각의 필드에 입력하고 **등록** 버튼을 클릭합니. 여기서 `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

   - **이름**: `spn-gppb{{랜덤숫자}}-apim`
   - **지원되는 계정 유형**: `이 조직 디렉터리의 계정만(Microsoft만 - 단일 테넌트)` 선택
   - **리디렉션 URI(선택 사항)**: `웹` ➡️ `https://oauth.pstmn.io/v1/callback`

     > **NOTE**: 여기서 `https://oauth.pstmn.io/v1/callback`는 이후 잠깐 테스트를 해 볼 [포스트맨](https://www.postman.com)의 콜백 URL입니다.

    ![애플리케이션 등록][image04]

5. 앱 등록 이후 **개요** 블레이드에서 **애플리케이션(클라이언트) ID** 값과 **디렉터리(테넌트) ID** 값을 복사합니다.

    ![애플리케이션 등록 확인 #1][image05]

6. **엔드포인트** 메뉴를 클릭한 후, **OAuth 2.0 권한 부여 엔드포인트(v2)** 값과 **OAuth 2.0 토큰 엔드포인트(v2)** 값을 복사해 둡니다. 반드시 **(v2)** 버전의 엔드포인트를 복사하세요.

    ![엔드포인트 확인][image06]

7. 방금 등록한 애플리케이션의 **인증** 블레이드를 클릭해서 등록한 내용을 확인합니다. 이 때 **리디렉션 URI** 항목에 **URI 추가** 버튼을 이용해 아래 두 URL을 추가한 후 저장합니다.

   - `https://oauth.pstmn.io/v1/browser-callback`
   - `https://global.consent.azure-apim.net/redirect`

    ![애플리케이션 등록 확인 #2][image07]

   > **NOTE**: 위에 추가한 두 콜백 URL은 웹 브라우저 기반 포스트맨과 파워 플랫폼 커스텀 커넥터에서 사용합니다. 지금 추가해 두지 않으면 나중에 테스트할 때 에러가 생길 수 있습니다.

8. **인증서 및 암호** 블레이드로 이동해서 새 클라이언트 암호를 등록해야 합니다. **클라이언트 비밀** ➡️ **➕ 새 클라이언트 암호** 메뉴를 클릭한 후 아래와 같이 입력합니다. 이후 **추가** 버튼을 클릭합니다.

   - **설명**: `apim` 입력
   - **만료 시간**: `Recommended: 180d dys (6 months)` 선택

    ![클라이언트 시크릿 추가][image08]

9. 아래 그림과 같이 클라이언트 시크릿이 만들어졌습니다. 이 값을 복사해서 어딘가에 저장해 둡니다. 이 화면을 벗어나는 순간 더이상 확인할 방법이 없으니 주의하세요!

    ![클라이언트 시크릿 확인][image09]

   이제 아래 값들을 복사해 둔 것을 반드시 기억하세요.

   - **디렉터리(테넌트) ID**
   - **애플리케이션(클라이언트) ID**
   - **클라이언트 암호(시크릿)**
   - **OAuth2 권한 부여 엔드포인트 (v2)**
   - **OAuth2 토큰 엔드포인트 (v2)**

10. **API 사용 권한** 블레이드로 이동해서 아래 그림과 같이 **API/권한 이름** 항목에 `Microsoft Graph/User.Read` 권한이 추가되었는지 확인하세요.

    ![API 사용 권한 확인][image10]

    만약 위와 같은 내용이 보이지 않는다면 **🔄 새로 고침** 메뉴를 클릭해 보세요. 만약 그래도 보이지 않는다면 아래와 같이 **➕ 권한 추가** 메뉴를 클릭해서 추가해야 합니다. **Microsoft Graph** ➡️ **위임된 권한** ➡️ **User** ➡️ **User.Read**를 선택한 후 **권한 추가** 버튼을 클릭합니다.

    ![API 사용 권한 추가][image11]

애저 액티브 디렉토리에 OAuth2 인증을 위한 애플리케이션을 등록했습니다.

## 2. API 앱 개발하기 ##

이미 최소한의 작동을 하는 API 앱이 [애저 펑션][az fncapp]으로 만들어져 있습니다. 이 API 앱을 애저 API 관리자에 연동하기 위한 작업으로 OpenAPI 문서 자동 생성 도구를 추가해 보겠습니다.

1. 아래 명령어를 통해 API 앱을 생성합니다.

    ```bash
    unzip ./custom-connectors-in-a-day/AuthCodeAuthApp.zip -d ./custom-connectors-in-a-day/src
    ```

2. 아래 명령어를 실행시켜 방금 생성한 API 앱을 솔루션에 연결시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day \
        && dotnet sln add ./src/AuthCodeAuthApp -s src \
        && popd
    ```

3. 아래 명령어를 실행시켜 GitHub 코드스페이스에서 API 앱을 실행시킬 수 있게끔 `local.settings.json` 파일을 생성합니다.

    ```bash
    pwsh -c "Invoke-RestMethod https://aka.ms/azfunc-openapi/add-codespaces.ps1 | Invoke-Expression"
    ```

4. 아래 명령어를 통해 API 앱을 실행시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day/src/AuthCodeAuthApp \
        && dotnet restore && dotnet build \
        && func start
    ```

5. 아래 팝업창이 나타나면 **Open in Browser** 버튼을 클릭합니다.

    ![새 창에서 API 열기 #1][image12]

6. 아래와 같은 화면이 나타나면 API 앱이 성공적으로 작동하는 것입니다.

    ![애저 펑션 앱 실행 결과 #1][image13]

7. 이제 주소창의 URL 맨 뒤에 `/api/profile`을 붙인후 아래와 같은 결과가 나오는지 확인합니다.

    ![애저 펑션 앱 API 호출 결과][image14]

8. 이제 터미널에서 `Control + C` 키를 눌러 애저 펑션 앱을 종료합니다.

9. 아래 명령어를 실행시켜 리포지토리의 루트 디렉토리로 돌아옵니다.

    ```bash
    popd
    ```

10. `custom-connectors-in-a-day/src/AuthCodeAuthApp/AuthCodeAuthApp.csproj` 파일을 열어 아래 부분의 주석을 제거합니다.

    ```xml
    ...
    <ItemGroup>
      <PackageReference Include="Microsoft.Azure.Functions.Extensions" Version="1.1.0" />
      <PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="6.0.1" />
      <PackageReference Include="Microsoft.Extensions.Http" Version="6.0.0" />
      <PackageReference Include="Microsoft.NET.Sdk.Functions" Version="4.1.3" />
  
      <!-- ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️ -->
      <PackageReference Include="Microsoft.Azure.WebJobs.Extensions.OpenApi" Version="1.*" />
      <!-- ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️ -->
    </ItemGroup>
    ...
    ```

11. `custom-connectors-in-a-day/src/AuthCodeAuthApp/Startup.cs` 파일을 열어 아래 부분의 주석을 제거합니다.

    ```csharp
    ...
    // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
    using Microsoft.OpenApi.Models;
    // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️
    ...
    private static void ConfigureAppSettings(IServiceCollection services)
    {
        // ⬇️⬇️⬇️ 아래의 코드 주석을 막아주세요 ⬇️⬇️⬇️
        // var settings = new GraphSettings();
        // services.BuildServiceProvider()
        //         .GetService<IConfiguration>()
        //         .GetSection(GraphSettings.Name)
        //         .Bind(settings);
        // services.AddSingleton(settings);
        // ⬆️⬆️⬆️ 위의 코드 주석을 막아주세요 ⬆️⬆️⬆️

        // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
        var settings = services.BuildServiceProvider()
                                .GetService<IConfiguration>()
                                .Get<GraphSettings>(GraphSettings.Name);
        services.AddSingleton(settings);

        var options = new DefaultOpenApiConfigurationOptions()
        {
            OpenApiVersion = OpenApiVersionType.V3,
            Info = new OpenApiInfo()
            {
                Version = "1.0.0",
                Title = "API AuthN'd by Authorization Code Auth",
                Description = "This is the API authN'd by Authorization Code Auth."
            }
        };

        var codespaces = bool.TryParse(Environment.GetEnvironmentVariable("OpenApi__RunOnCodespaces"), out var isCodespaces) && isCodespaces;
        if (codespaces)
        {
            options.IncludeRequestingHostName = false;
        }

        services.AddSingleton<IOpenApiConfigurationOptions>(options);
    }
    ...
    ```

12. `custom-connectors-in-a-day/src/AuthCodeAuthApp/AuthCodeAuthHttpTrigger.cs` 파일을 열어 아래 부분의 주석을 제거합니다.

    ```csharp
    ...
    // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
    using Microsoft.OpenApi.Models;
    // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️
    ...
    [FunctionName(nameof(ApiKeyAuthHttpTrigger.GetGreeting))]

    // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
    [OpenApiOperation(operationId: "Profile", tags: new[] { "profile" })]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GraphUser), Description = "The OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Description = "The bad request response")]
    // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️

    public async Task<IActionResult> GetProfile(
    ...
    ```

13. 아래 명령어를 통해 API 앱을 실행시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day/src/AuthCodeAuthApp \
        && dotnet build \
        && func start
    ```

14. 아래 팝업창이 나타나면 **Open in Browser** 버튼을 클릭합니다.

    ![새 창에서 API 열기 #2][image15]

15. 아래와 같은 화면이 나타나면 API 앱이 성공적으로 작동하는 것입니다.

    ![애저 펑션 앱 실행 결과 #2][image16]

16. 이제 주소창의 URL 맨 뒤에 `/api/swagger/ui`을 붙인후 아래와 같은 화면이 나오는지 확인합니다.

    ![애저 펑션 Swagger UI][image17]

17. 위 Swagger UI 화면에서 화살표가 가리키는 링크를 클릭합니다.

    ![애저 펑션 Swagger UI에서 swagger.json 문서 링크 클릭][image18]

18. 아래 그림과 같이 `swagger.json`라는 이름으로 OpenAPI 문서가 보이는지 확인합니다.

    ![애저 펑션 OpenAPI 문서 출력][image19]

19. 이제 터미널에서 `Control + C` 키를 눌러 애저 펑션 앱을 종료합니다.

20. 아래 명령어를 실행시켜 리포지토리의 루트 디렉토리로 돌아옵니다.

    ```bash
    popd
    ```

[애저 펑션][az fncapp]을 이용한 OAuth2 인증용 API 앱에 OpenAPI 기능을 추가하는 과정이 끝났습니다.


## 3. GitHub 액션 연동후 자동 배포하기 ##

앞서 개발한 API 앱을 GitHub 액션 워크플로우를 이용해 애저에 배포합니다. 아래 순서대로 따라해 보세요.

1. `custom-connectors-in-a-day/infra/gha-matrix.json` 파일을 열어 아래와 같이 수정합니다.

    ```jsonc
    [
      {
        "name": "apikeyauth",
        "suffix": "api-key-auth",
        "path": "ApiKeyAuthApp",
        "nv": "API_KEY_AUTH"
      },
      {
        "name": "basicauth",
        "suffix": "basic-auth",
        "path": "BasicAuthApp",
        "nv": "BASIC_AUTH"
      }, // 👈 쉼표 잊지 마세요

      // ⬇️⬇️⬇️ 아래 JSON 개체를 추가하세요 ⬇️⬇️⬇️
      {
        "name": "authcodeauth",
        "suffix": "auth-code-auth",
        "path": "AuthCodeAuthApp",
        "nv": "AUTH_CODE_AUTH"
      }
      // ⬆️⬆️⬆️ 위 JSON 개체를 추가하세요 ⬆️⬆️⬆️
    ]
    ```

2. 변경한 API 앱을 깃헙에 커밋합니다.

    ```bash
    git add . \
        && git commit -m "OAuth2 인증 앱 수정" \
        && git push origin
    ```

3. 아래와 같이 GitHub 액션 워크플로우가 자동으로 실행되는 것을 확인합니다.

    ![GitHub 액션 워크플로우 실행중][image20]

4. 아래와 같이 모든 GitHub 액션 워크플로우가 성공적으로 실행된 것을 확인합니다.

    ![GitHub 액션 워크플로우 실행 완료][image21]

5. 웹브라우저 주소창에 방금 배포한 API 앱의 주소를 입력하고 Swagger UI 화면이 나오는지 확인합니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```bash
    https://fncapp-gppb{{랜덤숫자}}-auth-code-auth.azurewebsites.net/api/swagger/ui
    ```

    ![배포된 API 앱 Swagger UI][image22]

6. 이제 아래 그림의 화살표가 가리키는 링크를 클릭해서 OpenAPI 문서를 표시합니다.

    ![배포된 API 앱 OpenAPI 문서 링크][image23]

7. OpenAPI 문서가 표시되는 것을 확인합니다.

    ![배포된 API 앱 OpenAPI 문서 생성][image24]

8. 이 OpenAPI 문서의 주소를 복사해 둡니다. 주소는 대략 아래와 같은 형식입니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```text
    https://fncapp-gppb{{랜덤숫자}}-auth-code-auth.azurewebsites.net/api/swagger.json
    ```

9. 이 API 앱을 테스트해 보겠습니다. [포스트맨](https://www.postman.com)에서 클라이언트 앱을 다운로드 받아 설치합니다.

   > **NOTE**: 클라이언트 앱을 실행시킬 때에는 로그인 여부와 상관 없이 사용할 수 있지만, 웹브라우저에서 직접 실행시킬 경우에는 로그인을 해야 합니다.

10. 아래와 같이 주소창에 입력합니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다. 주소창 앞에 **GET**으로 선택하는 것 확인하세요.

    ```text
    https://fncapp-gppb{{랜덤숫자}}-auth-code-auth.azurewebsites.net/api/profile
    ```

    ![Postman API 주소창 입력][image25]

11. **Authorization** 탭을 클릭한 후 아래와 같이 선택하고 값을 입력합니다. 이후 **Get New Access Token** 버튼을 클릭합니다.

    - **Type**: `OAuth 2.0` 선택
    - **Add authorization data to**: `Request Headers` 선택
    - **Token Name**: `AuthCodeAuth` 입력
    - **Grant Type**: `Authorization Code` 선택
    - **Callback URL**: `Authorize using browser` 체크
    - **Auth URL**: 앞서 복사해 둔 "OAuth 2.0 권한 부여 엔드포인트(v2)" 값 붙여넣기
    - **Access Token URL**: 앞서 복사해 둔 "OAuth 2.0 토큰 엔드포인트(v2)" 값 붙여넣기
    - **Client ID**: 앞서 복사해 둔 "애플리케이션(클라이언트) ID" 값 붙여넣기
    - **Client Secret**: 앞서 복사해 둔 "클라이언트 암호(시크릿)" 값 붙여넣기
    - **Scope**: `https://graph.microsoft.com/.default` 입력
    - **Client Authentication**: `Send as Basic Auth header` 선택

    ![액세스 토큰 설정][image26]

12. 아래와 같은 확인 창이 나타납니다. **동의함** 버튼을 클릭합니다.

    ![액세스 요청 확인][image27]

13. 아래 그림과 같이 액세스 토큰을 발급 받았습니다. **Use Token** 버튼을 클릭합니다.

    ![액세스 토큰 발급 확인][image28]

14. 방금 발급 받은 액세스 토큰이 Bearer 토큰 형식으로 **Authorization** 헤더에 들어간 것을 확인합니다. 이 값을 `Bearer` 부분을 포함해서 잘 복사해 둡니다.

    ![Authorization 헤더 확인][image29]

15. **Send** 버튼을 클릭해서 API를 호출합니다. 호출 결과가 `401 Unauthorized`인 것을 확인합니다. 이 에러는 다음에 이어지는 API 관리자 연동 과정에서 해결할 예정입니다.

    ![API 호출 실패 확인][image30]

[애저 펑션][az fncapp]을 이용한 OAuth2 인증용 API 앱 배포가 끝났습니다.


## 4. API 관리자 연동하기 ##

이번에는 [애저 API 관리자][az apim]에 방금 배포한 API 앱을 연동시켜 보겠습니다. 아래 순서대로 따라해 보세요.

1. 아래 명령어를 실행시켜 애저 펑션의 API Key를 받아옵니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```bash
    AZURE_ENV_NAME="gppb{{랜덤숫자}}"
    resgrp="rg-$AZURE_ENV_NAME"
    fncapp="fncapp-$AZURE_ENV_NAME-auth-code-auth"

    fncappKey=$(az functionapp keys list \
        -g $resgrp \
        -n $fncapp \
        --query "functionKeys.default" -o tsv)
    ```

2. 아래 명령어를 실행시켜 애저 펑션의 API Key를 API 관리자에 등록합니다.

    ```bash
    apim="apim-$AZURE_ENV_NAME"
    az apim nv create \
        -g $resgrp \
        -n $apim \
        --named-value-id "X-FUNCTIONS-KEY-AUTH-CODE-AUTH" \
        --display-name "X-FUNCTIONS-KEY-AUTH-CODE-AUTH" \
        --value $fncappKey \
        --secret true
    ```

3. 애저 포털의 API 관리자 화면에서 API 앱을 등록합니다. **API** ➡️ **➕ Add API** ➡️ **OpenAPI**를 선택하세요.

    ![OpenAPI 문서를 이용해 API 등록][image31]

4. 기본적으로 **Basic**이 선택되어 있는데, 이를 **Full**로 바꿉니다.
5. **OpenAPI specification** 필드에 앞서 복사해 둔 OpenAPI 문서 주소를 입력합니다. OpenAPI 문서 주소는 아래와 같습니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```text
    https://fncapp-gppb{{랜덤숫자}}-auth-code-auth.azurewebsites.net/api/swagger.json
    ```

6. **API URL suffix** 필드에 `authcodeauth`라고 입력합니다.
7. 마지막으로 **Create** 버튼을 클릭해서 API를 생성합니다.

    ![API 등록 과정][image32]

8. API 등록이 성공한 것을 확인합니다.

    ![API 등록 성공][image33]

9. 애저 펑션이 제공하는 API Key를 캡슐화시킵니다. 아래와 같이 **API AuthN'd by Authorization Code Auth** ➡️ **All operations** ➡️ **Inbound processing** ➡️ **Policies** 항목을 클릭합니다.

    ![API 인바운드 정책 등록][image34]

10. 화면에 보이는 XML 문서의 `policies/inbound` 노드 아래에 아래 내용을 입력한 후 저장합니다.

    ```xml
    <set-header name="x-functions-key" exists-action="override">
      <value>{{X-FUNCTIONS-KEY-AUTH-CODE-AUTH}}</value>
    </set-header>
    ```

    ![API 인바운드 정책 API Key 등록][image35]

11. 이제 API가 제대로 작동하는지 확인해 보겠습니다. 아래 그림과 같이 **API AuthN'd by Authorization Code Auth** ➡️ **Profile** ➡️ **Test** 화면으로 이동하세요. 이후 Header를 하나 추가합니다. **Name** 필드에 `Authorization`, **Value** 필드에 앞서 포스트맨 테스트 화면에서 복사했던 `Bearer ***` 인증 토큰을 입력합니다. 그리고 **Send** 버튼을 클릭하세요.

    ![API 테스트][image36]

12. 아래와 같이 테스트 결과가 나오는지 확인해 보세요.

    ![API 테스트 결과][image37]

[애저 API 관리자][az apim]에 [애저 펑션][az fncapp] API 앱을 연동시키는 작업이 끝났습니다.


## 5. 파워 플랫폼 커스텀 커넥터 생성하기 ##

이번에는 앞서 API 관리자에 등록한 API를 이용해 [파워 플랫폼 커스텀 커넥터][pp cuscon]를 만들어 보겠습니다. 아래 순서대로 따라해 보세요.

1. API 설정을 확인합니다. **API** ➡️ **API AuthN'd by Authorization Code Auth** ➡️ **Settings** 메뉴로 이동합니다.

    ![API 설정 화면][image38]

2. **Subscription required** 항목에 체크가 비활성화 되어 있는지 확인합니다. 체크가 되어 있다면 체크를 없애 비활성화한 후 저장합니다.

    ![API 구독 키 비활성화 확인][image39]

3. **API AuthN'd by Authorization Code Auth** 메뉴 옆에 있는 젬 세 개 버튼을 클릭한 후 **Export** 메뉴를 선택합니다. 그리고, **OpenAPI v2 (JSON)** 메뉴를 클릭합니다.

    ![OpenAPI v2 (JSON) 문서 내보내기][image40]

4. 컴퓨터에 아래와 같이 저장합니다. 기본 지정된 파일명은 `API AuthN'd by Authorization Code Auth.swagger.json`입니다.

    ![OpenAPI 문서 저장하기][image41]

5. 저장한 문서를 열어 아래와 같이 내용이 있는지 확인합니다.

   - OpenAPI 사양 버전
   - API 관리자 주소
   - 인증 방식 &ndash; `apiKeyHeader`, `apiKeyQuery`

    ```jsonc
    {
      // OpenAPI 사양 버전
      "swagger": "2.0",
      ...
      // API 관리자 주소
      "host": "apim-gppb{{랜덤숫자}}.azure-api.net",
      "basePath": "/authcodeauth",
      "schemes": [
        "https"
      ],
      // ⬇️⬇️⬇️ 인증 방식 지정 ⬇️⬇️⬇️
      "securityDefinitions": {
        "apiKeyHeader": {
          "type": "apiKey",
          "name": "Ocp-Apim-Subscription-Key",
          "in": "header"
        },
        "apiKeyQuery": {
          "type": "apiKey",
          "name": "subscription-key",
          "in": "query"
        }
      },
      "security": [
        {
          "apiKeyHeader": []
        },
        {
          "apiKeyQuery": []
        }
      ],
      // ⬆️⬆️⬆️ 인증 방식 지정 ⬆️⬆️⬆️
    ...
    ```

6. `securityDefinitions` 어트리뷰트와 `security` 어트리뷰트를 아래와 같이 `authcode_auth`로 바꾸고 저장합니다. 이 때 `{{TENANT_ID}}`는 앞서 [1. OAuth2 인증용 앱 등록하기](#1-oauth2-인증용-앱-등록하기)에서 저장했던 "디렉터리(테넌트) ID" 값으로 대체합니다.

    ```jsonc
    {
      ...
      // ⬇️⬇️⬇️ 인증 방식 지정 ⬇️⬇️⬇️
      "securityDefinitions": {
        "authcode_auth": {
          "type": "oauth2",
          "scopes": {
            "https://graph.microsoft.com/.default": "Default scope defined by Microsoft Graph"
          },
          "flow": "accessCode",
          "authorizationUrl": "https://login.microsoftonline.com/{{TENANT_ID}}/oauth2/v2.0/authorize",
          "tokenUrl": "https://login.microsoftonline.com/{{TENANT_ID}}/oauth2/v2.0/token"
        }
      },
      "security": [
        {
          "authcode_auth": [
            "https://graph.microsoft.com/.default"
          ]
        }
      ],
      // ⬆️⬆️⬆️ 인증 방식 지정 ⬆️⬆️⬆️
    ...
    ```

7. 파워 앱 또는 파워 오토메이트 앱을 실행시킵니다. 여기서는 편의상 파워 오토메이트로 합니다.

   - 파워 앱: [https://make.powerapps.com](https://make.powerapps.com)
   - 파워 오토메이트: [https://make.powerautomate.com](https://make.powerautomate.com)

8. **데이터** ➡️ **사용자 지정 커넥터** ➡️ **➕ 새 사용자 지정 커넥터** ➡️ **OpenAPI 파일 가져오기** 메뉴를 선택합니다.

    ![새 커스텀 커넥터 만들기][image42]

9. 아래와 같이 **커넥터 이름** 필드에 `AuthCode Auth`라고 입력하고, 앞서 저장했던 `API AuthN'd by Authorization Code Auth.swagger.json` 파일을 불러옵니다. 이후 **계속** 버튼을 클릭합니다.

    ![OpenAPI 문서 불러오기][image43]

10. 일반 정보 화면이 아래와 같이 보입니다. **2. 보안** 탭을 클릭합니다.

    ![커스텀 커넥터 읿반 정보][image44]

11. **인증 형식** 섹션 아래 **API로 구현되는 인증 선택** 필드 값이 `OAuth 2.0`, **OAuth 2.0** 섹션 아래 **ID 공급자** 필드 값이 `Generic Oauth 2`, **Authorization URL**, **Token URL** 필드 값이 각각 `https://login.microsoftonline.com/{{TENANT_ID}}/oauth2/v2.0/authorize`, `https://login.microsoftonline.com/{{TENANT_ID}}/oauth2/v2.0/token`와 같이 채워져 있는 것을 확인합니다. 여기서 `{{TENANT_ID}}`는 앞서 복사해 둔 "디렉터리(테넌트) ID" 값입니다. 마지막으로 **범위** 필드 값이 `https://graph.microsoft.com/.default`와 같이 들어 있는 것을 확인합니다.

    ![커스텀 커넥터 보안][image45]

12. 빠진 **Client id**, **Client secret**, **Refresh URL** 값을 아래와 같이 채워 넣습니다.

    - **Client id**: 앞서 복사해 둔 "애플리케이션(클라이언트) ID" 값
    - **Client secret**: 앞서 복사해 둔 "클라이언트 암호(시크릿)" 값
    - **Refresh URL**: **Token URL** 필드 값을 그대로 복사해서 붙여 넣기

    ![커스텀 커넥터 인증 정보 추가][image46]

13. **✔️ 커넥터 만들기** 버튼을 클릭해서 커스텀 커넥터 생성을 마무리합니다.

    ![커스텀 커넥터 만들기][image47]

14. 커스텀 커넥터가 만들어졌습니다. **5. 테스트** 메뉴를 클릭해서 이동합니다.

    ![커스텀 커넥터 생성후 테스트 메뉴 이동][image48]

15. 테스트 화면에서 **➕ 새 연결** 버튼을 클릭합니다.

    ![커스텀 커넥터 새 연결][image49]

16. 이미 **Client ID**, **Client Secret** 값을 커스텀 커넥터를 만들 때 입력해 두었기 때문에 곧바로 새 연결이 만들어지고 아래 그림과 같이 연결이 나타납니다. 만약 연결이 보이지 않는다면 새로 고침 버튼을 클릭합니다.

    ![커스텀 커넥터 연결 생성][image50]

17. 실제로 커스텀 커넥터가 작동하는지 확인해 보기 위해 아래와 같이 **테스트 작업** 버튼을 클릭합니다.

    ![커스텀 커넥터 연결 테스트][image51]

18. 커스텀 커넥터를 통해 API 호출이 성공적으로 이뤄진 것을 확인합니다.

    ![커스텀 커넥터 연결 테스트 성공][image52]

커스텀 커넥터를 만들고 이를 통해 API 앱을 호출하는 것까지 완성했습니다.


## 6. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기 ##

### 파워 오토메이트 ###

앞서 만들어 둔 커스텀 커넥터를 파워 오토메이트에서 사용해 보겠습니다. 아래 순서대로 따라해 보세요.

1. "인스턴트 클라우드 흐름"을 선택합니다.
2. **흐름 이름**은  `OAuth2 인증 플로우`라고 지정하고, **PowerApps(V2)** 트리거를 선택합니다.

    ![트리거 선택][image53]

3. 플로우 디자이너 화면에서 새 액션을 선택할 때 "사용자 지정" 탭을 클릭하면 **AuthCode Auth**라는 커스텀 커넥터가 보입니다. 이를 선택합니다.

    ![커스텀 커넥터 선택][image54]

4. **AuthCode Auth** 커스텀 커넥터에 포함되어 있는 **Profile** 액션을 선택합니다.

    ![커스텀 커넥터 액션 선택][image55]

   이 액션에서는 추가 정보가 필요하지 않으니 그냥 넘어갑니다.

5. 새 "응답" 액션을 추가합니다.

    ![응답 액션 추가][image56]

6. 아래 그림과 같이 각 필드에 값을 지정합니다.

    - **상태 코드**: `200`
    - **본문**: 동적 컨텐츠를 통해 **Profile** 액션의 `body`를 선택합니다.
    - **응답 본문 JSON 스키마**: 아래 내용을 입력합니다.

       ```json
       {
         "type": "object",
         "properties": {
           "@@odata.context": {
             "type": "string"
           },
           "id": {
             "type": "string"
           },
           "businessPhones": {
             "type": "array",
             "items": {
               "type": "string"
             }
           },
           "displayName": {
             "type": "string"
           },
           "givenName": {
             "type": "string"
           },
           "surname": {
             "type": "string"
           },
           "jobTitle": {
             "type": "string"
           },
           "mail": {
             "type": "string"
           },
           "mobilePhone": {},
           "officeLocation": {
             "type": "string"
           },
           "preferredLanguage": {},
           "userPrincipalName": {
             "type": "string"
           }
         }
       }
       ```

      ![응답 본문 추가][image57]

7. 파워 오토메이트 워크플로우를 저장한 후 테스트 버튼을 클릭해 플로우를 테스트합니다.

    ![파워 오토메이트 워크플로우 테스트 실행][image58]

8. 테스트 결과가 아래와 같이 보입니다.

    ![파워 오토메이트 워크플로우 실행 결과][image59]

파워 오토메이트에서 커스텀 커넥터를 통해 API를 잘 실행했습니다.


### 파워 앱 ###

이번에는 파워 앱에서 커스텀 커넥터를 사용해 보겠습니다. 커스텀 커넥터를 직접 파워 앱에 연결시킬 수도 있지만, 앞서 작성한 파워 오토메이트를 통해 실행시키는 방법으로 진행합니다. 아래 순서대로 따라해 보세요.

1. 빈 캔버스 앱을 하나 준비합니다. **앱 이름**은 `OAuth2 인증 앱`, **형식**은 `휴대폰`으로 설정합니다.
2. 캔버스에 **단추** 컨트롤 하나, **텍스트 레이블** 컨트롤 두개 추가합니다.

    ![파워 앱 캔버스에 컨트롤 추가][image60]

3. 캔버스 왼쪽의 파워 오토메이트 아이콘을 클릭한 후 **흐름 추가** ➡️ **Basic 인증 플로우**를 선택합니다.

    ![파워 앱에 파워 오토메이트 연결][image61]

4. **단추** 컨트롤을 선택한 후 화면 왼쪽 상단에 **OnSelect**를 선택합니다. 그리고 화면 가운데 상단에 아래와 같은 수식을 입력합니다.

    ```powerappsfl
    Set(
        profile,
        OAuth2인증플로우.Run()
    )
    ```

    ![파워 앱 단추 컨트롤에서 파워 오토메이트 호출][image62]

5. 첫번째 **텍스트 레이블** 컨트롤을 선택한 후 화면 왼쪽 상단에 **Text**를 선택합니다. 그리고 화면 가운데 상단에 아래와 같은 수식을 입력합니다.

    ```powerappsfl
    profile.displayName
    ```

    ![파워 앱 텍스트 레이블 컨트롤에서 파워 오토메이트 호출 결과 출력 #1][image63]

6. 두번째 **텍스트 레이블** 컨트롤을 선택한 후 화면 왼쪽 상단에 **Text**를 선택합니다. 그리고 화면 가운데 상단에 아래와 같은 수식을 입력합니다.

    ```powerappsfl
    profile.userPrincipalName
    ```

    ![파워 앱 텍스트 레이블 컨트롤에서 파워 오토메이트 호출 결과 출력 #2][image64]

7. 이 파워 앱을 실행시킵니다. 그러면 아래와 같이 자기 이름과 로그인 계정 이메일 주소를 보여주는 결과 화면이 나타납니다.

    ![파워 앱 실행 결과][image65]

8. 파워 앱을 저장하고 끝냅니다.

    ![파워 앱 저장][image66]

파워 앱에서 파워 오토메이트를 통해 커스텀 커넥터를 연결하고 API를 호출해 봤습니다.

---

여기까지 해서 OAuth2 인증을 통한 파워 플랫폼 커스텀 커넥터를 만들고, 이를 파워 앱과 파워 오토메이트에서 활용해 봤습니다.

- [애저 액티브 디렉토리 OAuth2 &ndash; 인증 코드 플로우][az ad oauth2 flow authcode]
- [파워 앱 더 알아보기][pp apps]
- [파워 오토메이트 더 알아보기][pp auto]

---

- 이전 세션: [Basic 인증 API 개발, 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./3-basic-auth.md)


[image01]: ./images/session04-image01.png
[image02]: ./images/session04-image02.png
[image03]: ./images/session04-image03.png
[image04]: ./images/session04-image04.png
[image05]: ./images/session04-image05.png
[image06]: ./images/session04-image06.png
[image07]: ./images/session04-image07.png
[image08]: ./images/session04-image08.png
[image09]: ./images/session04-image09.png
[image10]: ./images/session04-image10.png
[image11]: ./images/session04-image11.png
[image12]: ./images/session04-image12.png
[image13]: ./images/session04-image13.png
[image14]: ./images/session04-image14.png
[image15]: ./images/session04-image15.png
[image16]: ./images/session04-image16.png
[image17]: ./images/session04-image17.png
[image18]: ./images/session04-image18.png
[image19]: ./images/session04-image19.png
[image20]: ./images/session04-image20.png
[image21]: ./images/session04-image21.png
[image22]: ./images/session04-image22.png
[image23]: ./images/session04-image23.png
[image24]: ./images/session04-image24.png
[image25]: ./images/session04-image25.png
[image26]: ./images/session04-image26.png
[image27]: ./images/session04-image27.png
[image28]: ./images/session04-image28.png
[image29]: ./images/session04-image29.png
[image30]: ./images/session04-image30.png
[image31]: ./images/session04-image31.png
[image32]: ./images/session04-image32.png
[image33]: ./images/session04-image33.png
[image34]: ./images/session04-image34.png
[image35]: ./images/session04-image35.png
[image36]: ./images/session04-image36.png
[image37]: ./images/session04-image37.png
[image38]: ./images/session04-image38.png
[image39]: ./images/session04-image39.png
[image40]: ./images/session04-image40.png
[image41]: ./images/session04-image41.png
[image42]: ./images/session04-image42.png
[image43]: ./images/session04-image43.png
[image44]: ./images/session04-image44.png
[image45]: ./images/session04-image45.png
[image46]: ./images/session04-image46.png
[image47]: ./images/session04-image47.png
[image48]: ./images/session04-image48.png
[image49]: ./images/session04-image49.png
[image50]: ./images/session04-image50.png
[image51]: ./images/session04-image51.png
[image52]: ./images/session04-image52.png
[image53]: ./images/session04-image53.png
[image54]: ./images/session04-image54.png
[image55]: ./images/session04-image55.png
[image56]: ./images/session04-image56.png
[image57]: ./images/session04-image57.png
[image58]: ./images/session04-image58.png
[image59]: ./images/session04-image59.png
[image60]: ./images/session04-image60.png
[image61]: ./images/session04-image61.png
[image62]: ./images/session04-image62.png
[image63]: ./images/session04-image63.png
[image64]: ./images/session04-image64.png
[image65]: ./images/session04-image65.png
[image66]: ./images/session04-image66.png


[az ad]: https://learn.microsoft.com/ko-kr/azure/active-directory/fundamentals/active-directory-whatis?WT.mc_id=dotnet-87051-juyoo
[az ad oauth2]: https://learn.microsoft.com/ko-kr/azure/active-directory/fundamentals/auth-oauth2?WT.mc_id=dotnet-87051-juyoo
[az ad oauth2 flow authcode]: https://learn.microsoft.com/ko-kr/azure/active-directory/develop/v2-oauth2-auth-code-flow?WT.mc_id=dotnet-87051-juyoo

[az fncapp]: https://learn.microsoft.com/ko-kr/azure/azure-functions/functions-overview?WT.mc_id=dotnet-87051-juyoo

[az apim]: https://learn.microsoft.com/ko-kr/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-87051-juyoo

[pp apps]: https://learn.microsoft.com/ko-kr/power-apps/powerapps-overview?WT.mc_id=dotnet-87051-juyoo
[pp auto]: https://learn.microsoft.com/ko-kr/power-automate/getting-started?WT.mc_id=dotnet-87051-juyoo
[pp cuscon]: https://learn.microsoft.com/ko-kr/connectors/custom-connectors/?WT.mc_id=dotnet-87051-juyoo
