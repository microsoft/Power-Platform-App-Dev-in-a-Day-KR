# API Key 인증 API 활용 커스텀 커넥터 만들기 #

파워 플랫폼 커스텀 커넥터에 연동시키기 위한 API를 개발합니다. 이 API는 애저 API 관리자의 구독키를 이용해 한단계 더 인증을 추가합니다. 아래 순서대로 따라해 보세요.

- [API Key 인증 API 활용 커스텀 커넥터 만들기](#api-key-인증-api-활용-커스텀-커넥터-만들기)
  - [1. API 앱 개발하기](#1-api-앱-개발하기)
  - [2. GitHub 액션 연동후 자동 배포하기](#2-github-액션-연동후-자동-배포하기)
  - [3. API 관리자 연동하기](#3-api-관리자-연동하기)
  - [4. 파워 플랫폼 커스텀 커넥터 생성하기](#4-파워-플랫폼-커스텀-커넥터-생성하기)
  - [5. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기](#5-파워-앱과-파워-오토메이트에서-커스텀-커넥터-사용하기)


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

6. 아래와 같은 화면이 나타나면 API 앱을 성공적으로 실행시킨 것입니다.

    ![애저 펑션 앱 실행 결과 #1][image02]

7. 이제 주소창의 URL 맨 뒤에 `/api/greeting?name=GPPB`을 붙인후 아래와 같은 결과가 나오는지 확인합니다.

    ![애저 펑션 앱 API 호출 결과][image03]

8. 이제 터미널에서 `control + C` 키를 눌러 애저 펑션 앱을 종료합니다.

9. 아래 명령어를 실행시켜 리포지토리의 루트 디렉토리로 돌아옵니다.

    ```bash
    popd
    ```

10. `custom-connectors-in-a-day/src/ApiKeyAuthApp/ApiKeyAuthHttpTrigger.cs` 파일을 열어 아래 부분의 주석을 제거합니다.

    ```csharp
    ...
    [FunctionName(nameof(ApiKeyAuthHttpTrigger.GetGreeting))]

    // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
    [OpenApiOperation(operationId: "Greeting", tags: new[] { "greeting" })]
    [OpenApiSecurity("api_key", SecuritySchemeType.ApiKey, Name = "Ocp-Apim-Subscription-Key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GreetingResponse), Description = "The OK response")]
    // ⬆️⬆️⬆️ 위의 코드 주석을 풀어세요 ⬆️⬆️⬆️

    public async Task<IActionResult> GetGreeting(
    ...
    ```

11. 아래 명령어를 통해 API 앱을 실행시킵니다.

    ```bash
    pushd ./custom-connectors-in-a-day/src/ApiKeyAuthApp \
        && dotnet build \
        && func start
    ```

12. 아래 팝업창이 나타나면 **Open in Browser** 버튼을 클릭합니다.

    ![새 창에서 API 열기 #2][image04]

13. 아래와 같은 화면이 나타나면 API 앱을 성공적으로 실행시킨 것입니다.

    ![애저 펑션 앱 실행 결과 #2][image05]

14. 이제 주소창의 URL 맨 뒤에 `/api/swagger/ui`을 붙인후 아래와 같은 화면이 나오는지 확인합니다.

    ![애저 펑션 Swagger UI][image06]

15. 위 Swagger UI 화면에서 화살표가 가리키는 링크를 클릭합니다.

    ![애저 펑션 Swagger UI에서 swagger.json 문서 링크 클릭][image07]

16. 아래 그림과 같이 `swagger.json`라는 이름으로 OpenAPI 문서가 보이는지 확인합니다.

    ![애저 펑션 OpenAPI 문서 출력][image08]

17. 이제 터미널에서 `control + C` 키를 눌러 애저 펑션 앱을 종료합니다.

18. 아래 명령어를 실행시켜 리포지토리의 루트 디렉토리로 돌아옵니다.

    ```bash
    popd
    ```

애저 펑션을 이용한 API Key 인증용 API 앱에 OpenAPI 기능을 추가하는 과정이 끝났습니다.


## 2. GitHub 액션 연동후 자동 배포하기 ##

TBD


## 3. API 관리자 연동하기 ##

TBD


## 4. 파워 플랫폼 커스텀 커넥터 생성하기 ##

TBD


## 5. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기 ##

TBD


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


[az fncapp]: https://learn.microsoft.com/ko-kr/azure/azure-functions/functions-overview?WT.mc_id=dotnet-87051-juyoo
