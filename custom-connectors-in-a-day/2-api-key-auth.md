# API Key 인증 API 활용 커스텀 커넥터 만들기 #

파워 플랫폼 커스텀 커넥터에 연동시키기 위한 API를 개발합니다. 이 API는 애저 API 관리자의 구독키를 이용해 한단계 더 인증을 추가합니다. 아래 순서대로 따라해 보세요.

- [API Key 인증 API 활용 커스텀 커넥터 만들기](#api-key-인증-api-활용-커스텀-커넥터-만들기)
  - [1. API 앱 개발하기](#1-api-앱-개발하기)
  - [2. GitHub 액션 연동후 자동 배포하기](#2-github-액션-연동후-자동-배포하기)
  - [3. API 관리자 연동하기](#3-api-관리자-연동하기)
  - [4. 파워 플랫폼 커스텀 커넥터 생성하기](#4-파워-플랫폼-커스텀-커넥터-생성하기)
  - [5. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기](#5-파워-앱과-파워-오토메이트에서-커스텀-커넥터-사용하기)
    - [파워 오토메이트](#파워-오토메이트)
    - [파워 앱](#파워-앱)


## 1. API 앱 개발하기 ##

이미 최소한의 작동을 하는 API 앱이 [애저 펑션][az fncapp]으로 만들어져 있습니다. 이 API 앱을 애저 API 관리자에 연동하기 위한 작업으로 OpenAPI 문서 자동 생성 도구를 추가해 보겠습니다.

1. 아래 명령어를 통해 API 앱을 생성합니다.

    ```bash
    unzip ./custom-connectors-in-a-day/ApiKeyAuthApp.zip -d ./custom-connectors-in-a-day/src
    ```

2. 아래 명령어를 실행시켜 방금 생성한 API 앱을 솔루션에 연결시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day \
        && dotnet sln add ./src/ApiKeyAuthApp -s src \
        && popd
    ```

3. 아래 명령어를 실행시켜 GitHub 코드스페이스에서 API 앱을 실행시킬 수 있게끔 `local.settings.json` 파일을 생성합니다.

    ```bash
    pwsh -c "Invoke-RestMethod https://aka.ms/azfunc-openapi/add-codespaces.ps1 | Invoke-Expression"
    ```

4. 아래 명령어를 통해 API 앱을 실행시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day/src/ApiKeyAuthApp \
        && dotnet restore && dotnet build \
        && func start
    ```

5. 아래 팝업창이 나타나면 **Open in Browser** 버튼을 클릭합니다.

    ![새 창에서 API 열기 #1][image01]

6. 아래와 같은 화면이 나타나면 API 앱이 성공적으로 작동하는 것입니다.

    ![애저 펑션 앱 실행 결과 #1][image02]

7. 이제 주소창의 URL 맨 뒤에 `/api/greeting?name=GPPB`을 붙인후 아래와 같은 결과가 나오는지 확인합니다.

    ![애저 펑션 앱 API 호출 결과][image03]

8. 이제 터미널에서 `Control + C` 키를 눌러 애저 펑션 앱을 종료합니다.

9. 아래 명령어를 실행시켜 리포지토리의 루트 디렉토리로 돌아옵니다.

    ```bash
    popd
    ```

10. `custom-connectors-in-a-day/src/ApiKeyAuthApp/ApiKeyAuthApp.csproj` 파일을 열어 아래 부분의 주석을 제거합니다.

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

11. `custom-connectors-in-a-day/src/ApiKeyAuthApp/Startup.cs` 파일을 열어 아래 부분의 주석을 제거합니다.

    ```csharp
    ...
    // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
    using Microsoft.OpenApi.Models;
    // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️
    ...
    private static void ConfigureAppSettings(IServiceCollection services)
    {
        // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
        var options = new DefaultOpenApiConfigurationOptions()
        {
            OpenApiVersion = OpenApiVersionType.V3,
            Info = new OpenApiInfo()
            {
                Version = "1.0.0",
                Title = "API AuthN'd by API Key",
                Description = "This is the API authN'd by an API key."
            }
        };

        var codespaces = bool.TryParse(Environment.GetEnvironmentVariable("OpenApi__RunOnCodespaces"), out var isCodespaces) && isCodespaces;
        if (codespaces)
        {
            options.IncludeRequestingHostName = false;
        }

        services.AddSingleton<IOpenApiConfigurationOptions>(options);
        //⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️
    }
    ...
    ```

12. `custom-connectors-in-a-day/src/ApiKeyAuthApp/ApiKeyAuthHttpTrigger.cs` 파일을 열어 아래 부분의 주석을 제거합니다.

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
    [OpenApiOperation(operationId: "Greeting", tags: new[] { "greeting" })]
    [OpenApiSecurity("api_key", SecuritySchemeType.ApiKey, Name = "Ocp-Apim-Subscription-Key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GreetingResponse), Description = "The OK response")]
    // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️

    public async Task<IActionResult> GetGreeting(
    ...
    ```

13. 아래 명령어를 통해 API 앱을 실행시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day/src/ApiKeyAuthApp \
        && dotnet build \
        && func start
    ```

14. 아래 팝업창이 나타나면 **Open in Browser** 버튼을 클릭합니다.

    ![새 창에서 API 열기 #2][image04]

15. 아래와 같은 화면이 나타나면 API 앱이 성공적으로 작동하는 것입니다.

    ![애저 펑션 앱 실행 결과 #2][image05]

16. 이제 주소창의 URL 맨 뒤에 `/api/swagger/ui`을 붙인후 아래와 같은 화면이 나오는지 확인합니다.

    ![애저 펑션 Swagger UI][image06]

17. 위 Swagger UI 화면에서 화살표가 가리키는 링크를 클릭합니다.

    ![애저 펑션 Swagger UI에서 swagger.json 문서 링크 클릭][image07]

18. 아래 그림과 같이 `swagger.json`라는 이름으로 OpenAPI 문서가 보이는지 확인합니다.

    ![애저 펑션 OpenAPI 문서 출력][image08]

19. 이제 터미널에서 `Control + C` 키를 눌러 애저 펑션 앱을 종료합니다.

20. 아래 명령어를 실행시켜 리포지토리의 루트 디렉토리로 돌아옵니다.

    ```bash
    popd
    ```

[애저 펑션][az fncapp]을 이용한 API Key 인증용 API 앱에 OpenAPI 기능을 추가하는 과정이 끝났습니다.


## 2. GitHub 액션 연동후 자동 배포하기 ##

앞서 개발한 API 앱을 GitHub 액션 워크플로우를 이용해 애저에 배포합니다. 아래 순서대로 따라해 보세요.

1. 변경한 API 앱을 깃헙에 커밋합니다.

    ```bash
    git add . \
        && git commit -m "API Key 인증 앱 수정" \
        && git push origin
    ```

2. 아래와 같이 GitHub 액션 워크플로우가 자동으로 실행되는 것을 확인합니다.

    ![GitHub 액션 워크플로우 실행중][image09]

3. 아래와 같이 모든 GitHub 액션 워크플로우가 성공적으로 실행된 것을 확인합니다.

    ![GitHub 액션 워크플로우 실행 완료][image10]

4. 웹브라우저 주소창에 방금 배포한 API 앱의 주소를 입력하고 Swagger UI 화면이 나오는지 확인합니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```bash
    https://fncapp-gppb{{랜덤숫자}}-api-key-auth.azurewebsites.net/api/swagger/ui
    ```

    ![배포된 API 앱 Swagger UI][image11]

5. 아래 그림의 화살표가 가리키는 링크를 클릭해서 OpenAPI 문서를 표시합니다.

    ![배포된 API 앱 OpenAPI 문서 링크][image12]

6. OpenAPI 문서가 표시되는 것을 확인합니다.

    ![배포된 API 앱 OpenAPI 문서 생성][image13]

7. 이 OpenAPI 문서의 주소를 복사해 둡니다. 주소는 대략 아래와 같은 형식입니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```text
    https://fncapp-gppb{{랜덤숫자}}-api-key-auth.azurewebsites.net/api/swagger.json
    ```

[애저 펑션][az fncapp]을 이용한 API Key 인증용 API 앱 배포가 끝났습니다.


## 3. API 관리자 연동하기 ##

이번에는 [애저 API 관리자][az apim]에 방금 배포한 API 앱을 연동시켜 보겠습니다. 아래 순서대로 따라해 보세요.

1. 아래 명령어를 실행시켜 애저 펑션의 API Key를 받아옵니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```bash
    AZURE_ENV_NAME="gppb{{랜덤숫자}}"
    resgrp="rg-$AZURE_ENV_NAME"
    fncapp="fncapp-$AZURE_ENV_NAME-api-key-auth"

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
        --named-value-id "X-FUNCTIONS-KEY-API-KEY-AUTH" \
        --display-name "X-FUNCTIONS-KEY-API-KEY-AUTH" \
        --value $fncappKey \
        --secret true
    ```

3. 애저 포털의 API 관리자 화면에서 API 앱을 등록합니다. **API** ➡️ **➕ Add API** ➡️ **OpenAPI**를 선택하세요.

    ![OpenAPI 문서를 이용해 API 등록][image14]

4. 기본적으로 **Basic**이 선택되어 있는데, 이를 **Full**로 바꿉니다.
5. **OpenAPI specification** 필드에 앞서 복사해 둔 OpenAPI 문서 주소를 입력합니다. OpenAPI 문서 주소는 아래와 같습니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.

    ```text
    https://fncapp-gppb{{랜덤숫자}}-api-key-auth.azurewebsites.net/api/swagger.json
    ```

6. **API URL suffix** 필드에 `apikeyauth`라고 입력합니다.
7. 마지막으로 **Create** 버튼을 클릭해서 API를 생성합니다.

    ![API 등록 과정][image15]

8. API 등록이 성공한 것을 확인합니다.

    ![API 등록 성공][image16]

9. 애저 펑션이 제공하는 API Key를 캡슐화시킵니다. 아래와 같이 **API AuthN'd by API Key** ➡️ **All operations** ➡️ **Inbound processing** ➡️ **Policies** 항목을 클릭합니다.

    ![API 인바운드 정책 등록][image17]

10. 화면에 보이는 XML 문서의 `policies/inbound` 노드 아래에 아래 내용을 입력한 후 저장합니다.

    ```xml
    <set-header name="x-functions-key" exists-action="override">
      <value>{{X-FUNCTIONS-KEY-API-KEY-AUTH}}</value>
    </set-header>
    ```

    ![API 인바운드 정책 API Key 등록][image18]

11. 이제 API가 제대로 작동하는지 확인해 보겠습니다. 아래 그림과 같이 **API AuthN'd by API Key** ➡️ **Greeting** ➡️ **Test** 화면으로 이동하세요. 이후 **name** 필드에 값을 입력합니다. 여기서는 `GPPB`라고 입력했습니다. 그리고 **Send** 버튼을 클릭하세요.

    ![API 테스트][image19]

12. 아래와 같이 테스트 결과가 나오는지 확인해 보세요.

    ![API 테스트 결과][image20]

[애저 API 관리자][az apim]에 [애저 펑션][az fncapp] API 앱을 연동시키는 작업이 끝났습니다.


## 4. 파워 플랫폼 커스텀 커넥터 생성하기 ##

이번에는 앞서 API 관리자에 등록한 API를 이용해 [파워 플랫폼 커스텀 커넥터][pp cuscon]를 만들어 보겠습니다. 아래 순서대로 따라해 보세요.

1. 아래 명령어를 실행시켜 API 관리자의 구독 키 값을 받아옵니다. 커스텀 커넥터 작성 후 새 커넥션을 생성할 때 필요합니다.

    ```bash
    subscriptionId=$(az account show --query "id" -o tsv)
    apiVersion="2021-08-01"
    url="/subscriptions/$subscriptionId/resourceGroups/$resgrp/providers/Microsoft.ApiManagement/service/$apim/subscriptions/master/listSecrets"

    az rest --method post --url "$url?api-version=$apiVersion" --query "primaryKey" -o tsv
    ```

2. API 설정을 확인합니다. **API** ➡️ **API AuthN'd by API Key** ➡️ **Settings** 메뉴로 이동합니다.

    ![API 설정 화면][image21]

3. **Subscription required** 항목에 체크가 되어 있는지 확인합니다. 체크가 되어 있지 않다면 체크한 후 저장합니다.

    ![API 구독 키 설정 확인][image22]

4. **API AuthN'd by API Key** 메뉴 옆에 있는 젬 세 개 버튼을 클릭한 후 **Export** 메뉴를 선택합니다. 그리고, **OpenAPI v2 (JSON)** 메뉴를 클릭합니다.

    ![OpenAPI v2 (JSON) 문서 내보내기][image23]

5. 컴퓨터에 아래와 같이 저장합니다. 기본 지정된 파일명은 `API AuthN'd by API Key.swagger.json`입니다.

    ![OpenAPI 문서 저장하기][image24]

6. 저장한 문서를 열어 아래와 같이 내용이 있는지 확인합니다.

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
      "basePath": "/apikeyauth",
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

7. 파워 앱 또는 파워 오토메이트 앱을 실행시킵니다. 여기서는 편의상 파워 오토메이트로 합니다.

   - 파워 앱: [https://make.powerapps.com](https://make.powerapps.com)
   - 파워 오토메이트: [https://make.powerautomate.com](https://make.powerautomate.com)

8. **데이터** ➡️ **사용자 지정 커넥터** ➡️ **➕ 새 사용자 지정 커넥터** ➡️ **OpenAPI 파일 가져오기** 메뉴를 선택합니다.

    ![새 커스텀 커넥터 만들기][image25]

9. 아래와 같이 **커넥터 이름** 필드에 `API Key Auth`라고 입력하고, 앞서 저장했던 `API AuthN'd by API Key.swagger.json` 파일을 불러옵니다. 이후 **계속** 버튼을 클릭합니다.

    ![OpenAPI 문서 불러오기][image26]

10. 일반 정보 화면이 아래와 같이 보입니다. **2. 보안** 탭을 클릭합니다.

    ![커스텀 커넥터 읿반 정보][image27]

11. **인증 형식** 섹션 아래 **API로 구현되는 인증 선택** 필드 값이 `API 키`, **API 키** 섹션 아래 **매개 변수 레이블** 필드 값이 `API 키`, **매개 변수 이름** 필드 값이 `Ocp-Apim-Subscription-Key`, **매개 변수 위치** 필드 값이 `머리글`로 되어 있는 것을 확인합니다.

    ![커스텀 커넥터 보안][image28]

12. **✔️ 커넥터 만들기** 버튼을 클릭해서 커스텀 커넥터 생성을 마무리합니다.

    ![커스텀 커넥터 만들기][image29]

13. 커스텀 커넥터가 만들어졌습니다. **5. 테스트** 메뉴를 클릭해서 이동합니다.

    ![커스텀 커넥터 생성후 테스트 메뉴 이동][image30]

14. 테스트 화면에서 **➕ 새 연결** 버튼을 클릭합니다.

    ![커스텀 커넥터 새 연결][image31]

15. 새 창에서 **API 키** 입력창이 나타나면 앞서 받아놓은 API 관리자 구독 키 값을 입력합니다. 이후 **연결 만들기** 버튼을 클릭합니다.

    ![커스텀 커넥터 API 키 입력][image32]

16. 새 연결이 만들어지면 아래 그림과 같이 연결이 나타납니다. 만약 연결이 보이지 않는다면 새로 고침 버튼을 클릭합니다.

    ![커스텀 커넥터 연결 생성][image33]

17. 실제로 커스텀 커넥터가 작동하는지 확인해 보기 위해 아래와 같이 **Greeting** 섹션 아래 **name** 필드에 값을 입력합니다. 여기서는 `GPPB`라고 입력했습니다.

    ![커스텀 커넥터 연결 테스트][image34]

18. 커스텀 커넥터를 통해 API 호출이 성공적으로 이뤄진 것을 확인합니다.

    ![커스텀 커넥터 연결 테스트 성공][image35]

커스텀 커넥터를 만들고 이를 통해 API 앱을 호출하는 것까지 완성했습니다.


## 5. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기 ##

### 파워 오토메이트 ###

앞서 만들어 둔 커스텀 커넥터를 파워 오토메이트에서 사용해 보겠습니다. 아래 순서대로 따라해 보세요.

1. "인스턴트 클라우드 흐름"을 선택합니다.
2. **흐름 이름**은  `API Key 인증 플로우`라고 지정하고, **PowerApps(V2)** 트리거를 선택합니다.

    ![트리거 선택][image36]

3. **PowerApps(V2)** 트리거를 열어 **➕ 입력 추가**를 클릭합니다.

    ![트리거 입력 추가][image37]

4. 사용자 입력 종류를 **텍스트**로 선택합니다.

    ![트리거 입력 추가 - 사용자 입력 종류 선택][image38]

5. 필드 이름을 `name`으로 하고, 설명을 `이름`으로 지정합니다.

    ![트리거 입력 추가 - 사용자 입력 종류 필드명과 설명 입력][image39]

6. 플로우 디자이너 화면에서 새 액션을 선택할 때 "사용자 지정" 탭을 클릭하면 **API Key Auth**라는 커스텀 커넥터가 보입니다. 이를 선택합니다.

    ![커스텀 커넥터 선택][image40]

7. **API Key Auth** 커스텀 커넥터에 포함되어 있는 **Greeting** 액션을 선택합니다.

    ![커스텀 커넥터 액션 선택][image41]

8. **name** 필드에 값을 입력합니다. 동적 컨텐츠를 통해 파워 앱에서 받아오는 `name` 값을 사용합니다.

    ![커스텀 커넥터 액션 값 입력][image42]

9. 새 "응답" 액션을 추가합니다.

    ![응답 액션 추가][image43]

10. 아래 그림과 같이 각 필드에 값을 지정합니다.

    - **상태 코드**: `200`
    - **본문**: 동적 컨텐츠를 통해 **Greeting** 액션의 `body`를 선택합니다.
    - **응답 본문 JSON 스키마**: 아래 내용을 입력합니다.

       ```json
       {
         "type": "object",
         "properties": {
           "message": {
             "type": "string"
           }
         }
       }
       ```

      ![응답 본문 추가][image44]

11. 파워 오토메이트 워크플로우를 저장한 후 테스트 버튼을 클릭해 플로우를 테스트합니다.

    ![파워 오토메이트 워크플로우 테스트 실행][image45]

12. 테스트 수행 중, **name** 필드에는 임의의 값을 입력합니다. 여기서는 `GPPB`로 입력했습니다.

    ![파워 오토메이트 워크플로우 테스트 실행시 입력값][image46]

13. 테스트 결과가 아래와 같이 보입니다.

    ![파워 오토메이트 워크플로우 실행 결과][image47]

파워 오토메이트에서 커스텀 커넥터를 통해 API를 잘 실행했습니다.


### 파워 앱 ###

이번에는 파워 앱에서 커스텀 커넥터를 사용해 보겠습니다. 커스텀 커넥터를 직접 파워 앱에 연결시킬 수도 있지만, 앞서 작성한 파워 오토메이트를 통해 실행시키는 방법으로 진행합니다. 아래 순서대로 따라해 보세요.

1. 빈 캔버스 앱을 하나 준비합니다. **앱 이름**은 `API Key 인증 앱`, **형식**은 `휴대폰`으로 설정합니다.
2. 캔버스에 **텍스트 입력**, **단추**, **텍스트 레이블** 컨트롤을 각각 하나씩 추가합니다.

    ![파워 앱 캔버스에 컨트롤 추가][image48]

3. 캔버스 왼쪽의 파워 오토메이트 아이콘을 클릭한 후 **흐름 추가** ➡️ **API Key 인증 플로우**를 선택합니다.

    ![파워 앱에 파워 오토메이트 연결][image49]

4. **단추** 컨트롤을 선택한 후 화면 왼쪽 상단에 **OnSelect**를 선택합니다. 그리고 화면 가운데 상단에 아래와 같은 수식을 입력합니다.

    ```powerappsfl
    ClearCollect(
        greeting,
        APIKey인증플로우.Run(TextInput1.Text)
    )
    ```

    ![파워 앱 단추 컨트롤에서 파워 오토메이트 호출][image50]

5. **텍스트 레이블** 컨트롤을 선택한 후 화면 왼쪽 상단에 **Text**를 선택합니다. 그리고 화면 가운데 상단에 아래와 같은 수식을 입력합니다.

    ```powerappsfl
    First(greeting).message
    ```

    ![파워 앱 텍스트 레이블 컨트롤에서 파워 오토메이트 호출 결과 출력][image51]

6. 이 파워 앱을 실행시킨 후 텍스트 박스 컨트롤에 임의의 값을 입력합니다. 여기서는 `GPPB`라고 입력했습니다. 그러면 아래와 같은 결과 화면이 나타납니다.

    ![파워 앱 실행 결과][image52]

7. 파워 앱을 저장하고 끝냅니다.

    ![파워 앱 저장][image53]

파워 앱에서 파워 오토메이트를 통해 커스텀 커넥터를 연결하고 API를 호출해 봤습니다.

---

여기까지 해서 API Key 인증을 통한 파워 플랫폼 커스텀 커넥터를 만들고, 이를 파워 앱과 파워 오토메이트에서 활용해 봤습니다.

- [파워 앱 더 알아보기][pp apps]
- [파워 오토메이트 더 알아보기][pp auto]

---

- 이전 세션: [애저 Dev CLI 이용해서 애저 인스턴스 만들기](./1-azd.md)
- 다음 세션: [Basic 인증 API 개발, 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./3-basic-auth.md)


[image01]: ./images/session02-image01.png
[image02]: ./images/session02-image02.png
[image03]: ./images/session02-image03.png
[image04]: ./images/session02-image04.png
[image05]: ./images/session02-image05.png
[image06]: ./images/session02-image06.png
[image07]: ./images/session02-image07.png
[image08]: ./images/session02-image08.png
[image09]: ./images/session02-image09.png
[image10]: ./images/session02-image10.png
[image11]: ./images/session02-image11.png
[image12]: ./images/session02-image12.png
[image13]: ./images/session02-image13.png
[image14]: ./images/session02-image14.png
[image15]: ./images/session02-image15.png
[image16]: ./images/session02-image16.png
[image17]: ./images/session02-image17.png
[image18]: ./images/session02-image18.png
[image19]: ./images/session02-image19.png
[image20]: ./images/session02-image20.png
[image21]: ./images/session02-image21.png
[image22]: ./images/session02-image22.png
[image23]: ./images/session02-image23.png
[image24]: ./images/session02-image24.png
[image25]: ./images/session02-image25.png
[image26]: ./images/session02-image26.png
[image27]: ./images/session02-image27.png
[image28]: ./images/session02-image28.png
[image29]: ./images/session02-image29.png
[image30]: ./images/session02-image30.png
[image31]: ./images/session02-image31.png
[image32]: ./images/session02-image32.png
[image33]: ./images/session02-image33.png
[image34]: ./images/session02-image34.png
[image35]: ./images/session02-image35.png
[image36]: ./images/session02-image36.png
[image37]: ./images/session02-image37.png
[image38]: ./images/session02-image38.png
[image39]: ./images/session02-image39.png
[image40]: ./images/session02-image40.png
[image41]: ./images/session02-image41.png
[image42]: ./images/session02-image42.png
[image43]: ./images/session02-image43.png
[image44]: ./images/session02-image44.png
[image45]: ./images/session02-image45.png
[image46]: ./images/session02-image46.png
[image47]: ./images/session02-image47.png
[image48]: ./images/session02-image48.png
[image49]: ./images/session02-image49.png
[image50]: ./images/session02-image50.png
[image51]: ./images/session02-image51.png
[image52]: ./images/session02-image52.png
[image53]: ./images/session02-image53.png


[az fncapp]: https://learn.microsoft.com/ko-kr/azure/azure-functions/functions-overview?WT.mc_id=dotnet-87051-juyoo

[az apim]: https://learn.microsoft.com/ko-kr/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-87051-juyoo

[pp apps]: https://learn.microsoft.com/ko-kr/power-apps/powerapps-overview?WT.mc_id=dotnet-87051-juyoo
[pp auto]: https://learn.microsoft.com/ko-kr/power-automate/getting-started?WT.mc_id=dotnet-87051-juyoo
[pp cuscon]: https://learn.microsoft.com/ko-kr/connectors/custom-connectors/?WT.mc_id=dotnet-87051-juyoo