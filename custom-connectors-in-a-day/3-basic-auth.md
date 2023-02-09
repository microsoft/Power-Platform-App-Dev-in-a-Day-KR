# Basic 인증 API 활용 커스텀 커넥터 만들기 #

파워 플랫폼 커스텀 커넥터에 연동시키기 위한 API를 개발합니다. 이 API는 애저 API 관리자에서 지원하지 않는 Basic 인증을 구현합니다. 아래 순서대로 따라해 보세요.

- [Basic 인증 API 활용 커스텀 커넥터 만들기](#basic-인증-api-활용-커스텀-커넥터-만들기)
  - [1. Atlassian 계정 만들기](#1-atlassian-계정-만들기)
  - [2. API 앱 개발하기](#2-api-앱-개발하기)
  - [3. GitHub 액션 연동후 자동 배포하기](#3-github-액션-연동후-자동-배포하기)
  - [4. API 관리자 연동하기](#4-api-관리자-연동하기)
  - [5. 파워 플랫폼 커스텀 커넥터 생성하기](#5-파워-플랫폼-커스텀-커넥터-생성하기)
  - [6. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기](#6-파워-앱과-파워-오토메이트에서-커스텀-커넥터-사용하기)
    - [파워 오토메이트](#파워-오토메이트)
    - [파워 앱](#파워-앱)


## 1. Atlassian 계정 만들기 ##

이번에 개발하는 API는 Atlassian에 있는 내 계정 정보를 받아서 API 호출 결과를 반환합니다. 회사에서 Jira 또는 Confluence를 사용하고 있다면 아마도 계정이 있을 겁니다.

1. [Atlassian 웹사이트](https://www.atlassian.com/ko)에 로그인합니다. 계정이 없으면 새로 만드세요.
2. 웹 브라우저의 주소창에 보면 아래와 같이 보입니다. 내가 사용하는 Atlassian 인스턴스 이름이 `{{ATLASSIAN_INSTANCE_NAME}}`입니다.

    ```text
    https://{{ATLASSIAN_INSTANCE_NAME}}.atlassian.net
    ```

3. 이 API에서 사용하는 Basic 인증 정보는 아래와 같습니다.

   - email 주소
   - API 토큰

   API 토큰은 [이 문서](https://developer.atlassian.com/cloud/jira/platform/basic-auth-for-rest-apis/)를 참고해서 만드세요. 여기서는 다루지 않습니다.

Atlassian에 접속하기 위한 email 주소와 API 토큰을 발급 받았습니다.


## 2. API 앱 개발하기 ##

이미 최소한의 작동을 하는 API 앱이 [애저 펑션][az fncapp]으로 만들어져 있습니다. 이 API 앱을 애저 API 관리자에 연동하기 위한 작업으로 OpenAPI 문서 자동 생성 도구를 추가해 보겠습니다.

1. 아래 명령어를 통해 API 앱을 생성합니다.

    ```bash
    unzip ./custom-connectors-in-a-day/BasicAuthApp.zip -d ./custom-connectors-in-a-day/src
    ```

2. 아래 명령어를 실행시켜 방금 생성한 API 앱을 솔루션에 연결시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day \
        && dotnet sln add ./src/BasicAuthApp -s src \
        && popd
    ```

3. 아래 명령어를 실행시켜 GitHub 코드스페이스에서 API 앱을 실행시킬 수 있게끔 `local.settings.json` 파일을 생성합니다.

    ```bash
    pwsh -c "Invoke-RestMethod https://aka.ms/azfunc-openapi/add-codespaces.ps1 | Invoke-Expression"
    ```

4. `loca.settings.json` 파일을 열어 아래 부분을 수정합니다. 앞서 기록해 둔 `{{ATLASSIAN_INSTANCE_NAME}}`값으로 대체합니다.

    ```jsonc
    "Atlassian__InstanceName": "{{ATLASSIAN_INSTANCE_NAME}}",
    ```

5. 아래 명령어를 통해 API 앱을 실행시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day/src/BasicAuthApp \
        && dotnet restore && dotnet build \
        && func start
    ```

6. 아래 팝업창이 나타나면 **Open in Browser** 버튼을 클릭합니다.

    ![새 창에서 API 열기 #1][image01]

7. 아래와 같은 화면이 나타나면 API 앱이 성공적으로 작동하는 것입니다.

    ![애저 펑션 앱 실행 결과 #1][image02]

8. 이제 주소창의 URL 맨 뒤에 `/api/profile`을 붙인후 아래와 같은 결과가 나오는지 확인합니다.

    ![애저 펑션 앱 API 호출 결과][image03]

9. 이제 터미널에서 `Control + C` 키를 눌러 애저 펑션 앱을 종료합니다.

10. 아래 명령어를 실행시켜 리포지토리의 루트 디렉토리로 돌아옵니다.

    ```bash
    popd
    ```

11. `custom-connectors-in-a-day/src/BasicAuthApp/BasicAuthApp.csproj` 파일을 열어 아래 부분의 주석을 제거합니다.

    ```xml
    ...
    <ItemGroup>
      <PackageReference Include="Microsoft.Azure.Functions.Extensions" Version="1.1.0" />
      <PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="6.0.1" />
      <PackageReference Include="Microsoft.Extensions.Http" Version="6.0.0" />
      <PackageReference Include="Microsoft.NET.Sdk.Functions" Version="4.1.3" />

      <!-- ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️ -->
      <!-- <PackageReference Include="Microsoft.Azure.WebJobs.Extensions.OpenApi" Version="1.*" /> -->
      <!-- ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️ -->
    </ItemGroup>
    ...
    ```

12. `custom-connectors-in-a-day/src/BasicAuthApp/Startup.cs` 파일을 열어 아래 부분의 주석을 제거합니다.

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
        // var settings = new AtlassianSettings();
        // services.BuildServiceProvider()
        //         .GetService<IConfiguration>()
        //         .GetSection(AtlassianSettings.Name)
        //         .Bind(settings);
        // services.AddSingleton(settings);
        // ⬆️⬆️⬆️ 위의 코드 주석을 막아주세요 ⬆️⬆️⬆️

        // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
        var settings = services.BuildServiceProvider()
                               .GetService<IConfiguration>()
                               .Get<AtlassianSettings>(AtlassianSettings.Name);
        services.AddSingleton(settings);

        var options = new DefaultOpenApiConfigurationOptions()
        {
            OpenApiVersion = OpenApiVersionType.V3,
            Info = new OpenApiInfo()
            {
                Version = "1.0.0",
                Title = "API AuthN'd by Basic Auth",
                Description = "This is the API authN'd by Basic Auth."
            }
        };

        var codespaces = bool.TryParse(Environment.GetEnvironmentVariable("OpenApi__RunOnCodespaces"), out var isCodespaces) && isCodespaces;
        if (codespaces)
        {
            options.IncludeRequestingHostName = false;
        }

        services.AddSingleton<IOpenApiConfigurationOptions>(options);
        // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️
    }
    ...
    ```

13. `custom-connectors-in-a-day/src/ApiKeyAuthApp/ApiKeyAuthHttpTrigger.cs` 파일을 열어 아래 부분의 주석을 제거합니다.

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
    [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AtlassianUser), Description = "The OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Description = "The bad request response")]
    // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️

    public async Task<IActionResult> GetProfile(
    ...
    ```

14. 아래 명령어를 통해 API 앱을 실행시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day/src/BasicAuthApp \
        && dotnet build \
        && func start
    ```

15. 아래 팝업창이 나타나면 **Open in Browser** 버튼을 클릭합니다.

    ![새 창에서 API 열기 #2][image04]

16. 아래와 같은 화면이 나타나면 API 앱이 성공적으로 작동하는 것입니다.

    ![애저 펑션 앱 실행 결과 #2][image05]

17. 이제 주소창의 URL 맨 뒤에 `/api/swagger/ui`을 붙인후 아래와 같은 화면이 나오는지 확인합니다.

    ![애저 펑션 Swagger UI][image06]

18. 위 Swagger UI 화면에서 화살표가 가리키는 링크를 클릭합니다.

    ![애저 펑션 Swagger UI에서 swagger.json 문서 링크 클릭][image07]

19. 아래 그림과 같이 `swagger.json`라는 이름으로 OpenAPI 문서가 보이는지 확인합니다.

    ![애저 펑션 OpenAPI 문서 출력][image08]

20. 이제 터미널에서 `Control + C` 키를 눌러 애저 펑션 앱을 종료합니다.

21. 아래 명령어를 실행시켜 리포지토리의 루트 디렉토리로 돌아옵니다.

    ```bash
    popd
    ```

[애저 펑션][az fncapp]을 이용한 API Key 인증용 API 앱에 OpenAPI 기능을 추가하는 과정이 끝났습니다.


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
      }, // 👈 쉼표 잊지 마세요

      // ⬇️⬇️⬇️ 아래 JSON 개체를 추가하세요 ⬇️⬇️⬇️
      {
        "name": "basicauth",
        "suffix": "basic-auth",
        "path": "BasicAuthApp",
        "nv": "BASIC_AUTH"
      }
      // ⬆️⬆️⬆️ 위 JSON 개체를 추가하세요 ⬆️⬆️⬆️
    ]
    ```

2. 변경한 API 앱을 깃헙에 커밋합니다.

    ```bash
    git add . \
        && git commit -m "Basic 인증 앱 수정" \
        && git push origin
    ```

3. 아래와 같이 GitHub 액션 워크플로우가 자동으로 실행되는 것을 확인합니다.

    ![GitHub 액션 워크플로우 실행중][image09]

4. 아래와 같이 모든 GitHub 액션 워크플로우가 성공적으로 실행된 것을 확인합니다.

    ![GitHub 액션 워크플로우 실행 완료][image10]

5. 웹브라우저 주소창에 방금 배포한 API 앱의 주소를 입력하고 Swagger UI 화면이 나오는지 확인합니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```bash
    https://fncapp-gppb{{랜덤숫자}}-basic-auth.azurewebsites.net/api/swagger/ui
    ```

    ![배포된 API 앱 Swagger UI][image11]

6. 아래 그림과 같이 **Authorize** 버튼을 클릭합니다.

    ![인증 버튼 클릭][image12]

7. 앞서 생성한 Atlassian 계정의 이메일 주소와 API 토큰을 입력한 후 **Authorize** 버튼을 클릭합니다.

    ![인증 정보 입력][image13]

8. 아래와 같이 자물쇠 모양이 잠긴 것을 확인한 후 **Try it out** 버튼을 클릭합니다.

    ![API 테스트][image14]

9. 이후 **Execute** 버튼을 클릭하면 아래와 같이 `401 Unauthorized` 에러가 나오는 것을 확인하세요.

    ![API 테스트 결과][image15]

10. 아래와 같이 요청 헤더 부분의 `Basic ***`으로 시작하는 문자열을 복사해서 보관합니다. 이후 API 관리자 연동후 테스트 용도로 사용할 인증 키 값입니다.

    ![Basic 인증 키 값 복사][image16]

11. 이제 아래 그림의 화살표가 가리키는 링크를 클릭해서 OpenAPI 문서를 표시합니다.

    ![배포된 API 앱 OpenAPI 문서 링크][image17]

12. OpenAPI 문서가 표시되는 것을 확인합니다.

    ![배포된 API 앱 OpenAPI 문서 생성][image18]

13. 이 OpenAPI 문서의 주소를 복사해 둡니다. 주소는 대략 아래와 같은 형식입니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```text
    https://fncapp-gppb{{랜덤숫자}}-basic-auth.azurewebsites.net/api/swagger.json
    ```

14. 아래 명령어를 실행시켜 애저 펑션의 환경 변수 값을 업데이트합니다. `{{ATLASSIAN_INSTANCE_NAME}}` 값을 앞서 기록해 둔 내 Atlassian 인스턴스 이름으로 대체합니다.

    ```bash
    ATLASSIAN_INSTANCE_NAME="{{ATLASSIAN_INSTANCE_NAME}}"
    AZURE_ENV_NAME="gppb{{랜덤숫자}}"
    resgrp="rg-$AZURE_ENV_NAME"
    fncapp="fncapp-$AZURE_ENV_NAME-basic-auth"

    az functionapp config appsettings set \
        -g $resgrp \
        -n $fncapp \
        --settings Atlassian__InstanceName=$ATLASSIAN_INSTANCE_NAME
    ```

[애저 펑션][az fncapp]을 이용한 API 앱 배포가 끝났습니다.


## 4. API 관리자 연동하기 ##

이번에는 [애저 API 관리자][az apim]에 방금 배포한 API 앱을 연동시켜 보겠습니다. 아래 순서대로 따라해 보세요.

1. 아래 명령어를 실행시켜 애저 펑션의 API Key를 받아옵니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```bash
    AZURE_ENV_NAME="gppb{{랜덤숫자}}"
    resgrp="rg-$AZURE_ENV_NAME"
    fncapp="fncapp-$AZURE_ENV_NAME-basic-auth"

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
        --named-value-id "X-FUNCTIONS-KEY-BASIC-AUTH" \
        --display-name "X-FUNCTIONS-KEY-BASIC-AUTH" \
        --value $fncappKey \
        --secret true
    ```

3. 애저 포털의 API 관리자 화면에서 API 앱을 등록합니다. "API" ➡️ "+ Add API" ➡️ "OpenAPI"를 선택하세요.

    ![OpenAPI 문서를 이용해 API 등록][image19]

4. 기본적으로 **Basic**이 선택되어 있는데, 이를 **Full**로 바꿉니다.
5. **OpenAPI specification** 필드에 앞서 복사해 둔 OpenAPI 문서 주소를 입력합니다. OpenAPI 문서 주소는 아래와 같습니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```text
    https://fncapp-gppb{{랜덤숫자}}-basic-auth.azurewebsites.net/api/swagger.json
    ```

6. **API URL suffix** 필드에 `basicauth`라고 입력합니다.
7. 마지막으로 **Create** 버튼을 클릭해서 API를 생성합니다.

    ![API 등록 과정][image20]

8. API 등록이 성공한 것을 확인합니다.

    ![API 등록 성공][image21]

9. 애저 펑션이 제공하는 API Key를 캡슐화시킵니다. 아래와 같이 "API AuthN'd by Basic Auth" ➡️ "All operations" ➡️ "Inbound processing" ➡️ "Policies" 항목을 클릭합니다.

    ![API 인바운드 정책 등록][image22]

10. 화면에 보이는 XML 문서의 `policies/inbound` 노드 아래에 아래 내용을 입력한 후 저장합니다.

    ```xml
    <set-header name="x-functions-key" exists-action="override">
      <value>{{X-FUNCTIONS-KEY-BASIC-AUTH}}</value>
    </set-header>
    ```

    ![API 인바운드 정책 API Key 등록][image23]

11. 이제 API가 제대로 작동하는지 확인해 보겠습니다. 아래 그림과 같이 "API AuthN'd by Basic Auth" ➡️ "Profile" ➡️ "Test" 화면으로 이동하세요. 이후 Header를 하나 추가합니다. **Name** 필드에 `Authorization`, **Value** 필드에 앞서 Swagger UI 화면에서 복사했던 `Basic ***` 인증 토큰을 입력합니다. 그리고 **Send** 버튼을 클릭하세요.

    ![API 테스트][image24]

12. 아래와 같이 테스트 결과가 나오는지 확인해 보세요.

    ![API 테스트 결과][image25]

[애저 API 관리자][az apim]에 [애저 펑션][az fncapp] API 앱을 연동시키는 작업이 끝났습니다.


## 5. 파워 플랫폼 커스텀 커넥터 생성하기 ##

이번에는 앞서 API 관리자에 등록한 API를 이용해 [파워 플랫폼 커스텀 커넥터][pp cuscon]를 만들어 보겠습니다. 아래 순서대로 따라해 보세요.

1. API 설정을 확인합니다. "API" ➡️ "API AuthN'd by Basic Auth" ➡️ "Settings" 메뉴로 이동합니다.

    ![API 설정 화면][image26]

2. **Subscription required** 항목에 체크가 비활성화 되어 있는지 확인합니다. 체크가 되어 있다면 체크를 없애 비활성화한 후 저장합니다.

    ![API 구독 키 비활성화 확인][image27]

3. "API AuthN'd by Basic Auth" 메뉴 옆에 있는 젬 세 개 버튼을 클릭한 후 "Export" 메뉴를 선택합니다. 그리고, "OpenAPI v2 (JSON)" 메뉴를 클릭합니다.

    ![OpenAPI v2 (JSON) 문서 내보내기][image28]

4. 컴퓨터에 아래와 같이 저장합니다. 기본 지정된 파일명은 `API AuthN'd by Basic Auth.swagger.json`입니다.

    ![OpenAPI 문서 저장하기][image29]

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
      "basePath": "/basicauth",
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

6. `securityDefinitions` 어트리뷰트와 `security` 어트리뷰트를 아래와 같이 `basic_auth`로 바꾸고 저장합니다.

    ```jsonc
    {
      ...
      // ⬇️⬇️⬇️ 인증 방식 지정 ⬇️⬇️⬇️
      "securityDefinitions": {
        "basic_auth": {
          "type": "basic"
        }
      },
      "security": [
        {
          "basic_auth": []
        }
      ],
      // ⬆️⬆️⬆️ 인증 방식 지정 ⬆️⬆️⬆️
    ...
    ```

7. 파워 앱 또는 파워 오토메이트 앱을 실행시킵니다. 여기서는 편의상 파워 오토메이트로 합니다.

   - 파워 앱: [https://make.powerapps.com](https://make.powerapps.com)
   - 파워 오토메이트: [https://make.powerautomate.com](https://make.powerautomate.com)

8. "데이터" ➡️ "사용자 지정 커넥터" ➡️ "+ 새 사용자 지정 커넥터" ➡️ "OpenAPI 파일 가져오기" 메뉴를 선택합니다.

    ![새 커스텀 커넥터 만들기][image30]

9. 아래와 같이 **커넥터 이름** 필드에 `Basic Auth`라고 입력하고, 앞서 저장했던 `API AuthN'd by Basic Auth.swagger.json` 파일을 불러옵니다. 이후 **계속** 버튼을 클릭합니다.

    ![OpenAPI 문서 불러오기][image31]

10. 일반 정보 화면이 아래와 같이 보입니다. **2. 보안** 탭을 클릭합니다.

    ![커스텀 커넥터 읿반 정보][image32]

11. **인증 형식** 섹션 아래 **API로 구현되는 인증 선택** 필드 값이 `기본 인증`, **기본 인증** 섹션 아래 **매개 변수 레이블** 필드 값이 `사용자 이름`, `암호`로 되어 있는 것을 확인합니다.

    ![커스텀 커넥터 보안][image33]

12. **✔️ 커넥터 만들기** 버튼을 클릭해서 커스텀 커넥터 생성을 마무리합니다.

    ![커스텀 커넥터 만들기][image34]

13. 커스텀 커넥터가 만들어졌습니다. **5. 테스트** 메뉴를 클릭해서 이동합니다.

    ![커스텀 커넥터 생성후 테스트 메뉴 이동][image35]

14. 테스트 화면에서 **+ 새 연결** 버튼을 클릭합니다.

    ![커스텀 커넥터 새 연결][image36]

15. 새 창에서 **사용자 이름**, **암호** 입력창이 나타나면 앞서 받아놓은 Atlassian의 로그인 email 주소와 API 토큰을 입력합니다. 이후 **연결 만들기** 버튼을 클릭합니다.

    ![커스텀 커넥터 API 키 입력][image37]

16. 새 연결이 만들어지면 아래 그림과 같이 연결이 나타납니다. 만약 연결이 보이지 않는다면 새로 고침 버튼을 클릭합니다.

    ![커스텀 커넥터 연결 생성][image38]

17. 실제로 커스텀 커넥터가 작동하는지 확인해 보기 위해 아래와 같이 **테스트 작업** 버튼을 클릭합니다.

    ![커스텀 커넥터 연결 테스트][image39]

18. 커스텀 커넥터를 통해 API 호출이 성공적으로 이뤄진 것을 확인합니다.

    ![커스텀 커넥터 연결 테스트 성공][image40]

커스텀 커넥터를 만들고 이를 통해 API 앱을 호출하는 것까지 완성했습니다.



## 6. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기 ##

TBD


### 파워 오토메이트 ###

TBD


### 파워 앱 ###

TBD



파워 앱에서 파워 오토메이트를 통해 커스텀 커넥터를 연결하고 API를 호출해 봤습니다.

---

여기까지 해서 API Key 인증을 통한 파워 플랫폼 커스텀 커넥터를 만들고, 이를 파워 앱과 파워 오토메이트에서 활용해 봤습니다.

- [파워 앱 더 알아보기][pp apps]
- [파워 오토메이트 더 알아보기][pp auto]

---

- 이전 세션: [애저 Dev CLI 이용해서 애저 인스턴스 만들기](./1-azd.md)
- 다음 세션: [Basic 인증 API 개발, 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./2-api-key-auth.md)


[image01]: ./images/session03-image01.png
[image02]: ./images/session03-image02.png
[image03]: ./images/session03-image03.png
[image04]: ./images/session03-image04.png
[image05]: ./images/session03-image05.png
[image06]: ./images/session03-image06.png
[image07]: ./images/session03-image07.png
[image08]: ./images/session03-image08.png
[image09]: ./images/session03-image09.png
[image10]: ./images/session03-image10.png
[image11]: ./images/session03-image11.png
[image12]: ./images/session03-image12.png
[image13]: ./images/session03-image13.png
[image14]: ./images/session03-image14.png
[image15]: ./images/session03-image15.png
[image16]: ./images/session03-image16.png
[image17]: ./images/session03-image17.png
[image18]: ./images/session03-image18.png
[image19]: ./images/session03-image19.png
[image20]: ./images/session03-image20.png
[image21]: ./images/session03-image21.png
[image22]: ./images/session03-image22.png
[image23]: ./images/session03-image23.png
[image24]: ./images/session03-image24.png
[image25]: ./images/session03-image25.png
[image26]: ./images/session03-image26.png
[image27]: ./images/session03-image27.png
[image28]: ./images/session03-image28.png
[image29]: ./images/session03-image29.png
[image30]: ./images/session03-image30.png
[image31]: ./images/session03-image31.png
[image32]: ./images/session03-image32.png
[image33]: ./images/session03-image33.png
[image34]: ./images/session03-image34.png
[image35]: ./images/session03-image35.png
[image36]: ./images/session03-image36.png
[image37]: ./images/session03-image37.png
[image38]: ./images/session03-image38.png
[image39]: ./images/session03-image39.png
[image40]: ./images/session03-image40.png
[image41]: ./images/session03-image41.png
[image42]: ./images/session03-image42.png
[image43]: ./images/session03-image43.png
[image44]: ./images/session03-image44.png
[image45]: ./images/session03-image45.png
[image46]: ./images/session03-image46.png
[image47]: ./images/session03-image47.png
[image48]: ./images/session03-image48.png
[image49]: ./images/session03-image49.png
[image50]: ./images/session03-image50.png
[image51]: ./images/session03-image51.png
[image52]: ./images/session03-image52.png
[image53]: ./images/session03-image53.png


[az fncapp]: https://learn.microsoft.com/ko-kr/azure/azure-functions/functions-overview?WT.mc_id=dotnet-87051-juyoo

[az apim]: https://learn.microsoft.com/ko-kr/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-87051-juyoo

[pp apps]: https://learn.microsoft.com/ko-kr/power-apps/powerapps-overview?WT.mc_id=dotnet-87051-juyoo
[pp auto]: https://learn.microsoft.com/ko-kr/power-automate/getting-started?WT.mc_id=dotnet-87051-juyoo
[pp cuscon]: https://learn.microsoft.com/ko-kr/connectors/custom-connectors/?WT.mc_id=dotnet-87051-juyoo
