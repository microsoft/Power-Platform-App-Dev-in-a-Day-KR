# Session 03: 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기

앞서 생성한 [애저 API 관리자][az apim]를 이용해 커스텀 커넥터를 만들기 위해서는 백엔드 API를 API 관리자에 먼저 등록해야 합니다. 이번 세션에서는 API 관리자에 백엔드 API를 등록하고, 커스텀 커넥터를 만들어 보겠습니다.

## 1. API 관리자에 백엔드 API 등록하기

1. 아래 명령어를 통해 작업 디렉토리로 이동합니다. 이곳이 이번 세션에서 사용할 디렉토리입니다.

   ```bash
   cd $CODESPACE_VSCODE_FOLDER/devops-in-a-day
   ```

1. 아래 명령어를 실행시켜 백엔드 API 프로젝트를 실행합니다.

    ```bash
    dotnet watch run --project ./src/ApiApp
    ```

   > **NOTE**:
   > 
   > - 만약 브라우저의 새 탭이 열리지 않는다면, 웹 브라우저의 팝업창을 허용해 주세요.
   > - 만약 브라우저의 새 탭이 열려서 Swagger UI가 열리지 않았다면 주소창의 맨 마지막에 `/swagger`를 추가한 후 다시 실행시킵니다.

1. Swagger UI가 열리면 아래와 같이 링크를 클릭합니다.

    ![Swagger UI][image-01]

1. 아래와 같이 OpenAPI 문서가 열리는 것을 확인합니다. 이 문서의 URL을 복사합니다.

    ![OpenAPI 문서][image-02]

1. GitHub 코드스페이스로 돌아가 아래와 같이 현재 실행 중인 백엔드 API의 포트 Visibility를 `Private`에서 `Public`으로 변경합니다.

    ![포트 변경][image-03]

1. 애저 포털로 이동해서 앞서 생성한 API 관리자 리소스로 이동합니다.

    ![API 관리자][image-04]

1. API 관리자 리소스의 왼쪽 메뉴에서 **APIs 블레이드** 👉 `➕ Add API` 👉 `OpenAPI` 버튼을 클릭합니다.

    ![API 관리자 - APIs 블레이드][image-05]

1. 아래와 같이 내용을 입력한 후 `Create` 버튼을 클릭합니다.
   - `Full` 선택
   - OpenAPI Specification: 앞서 복사한 OpenAPI 문서의 URL
   - Display name: `GPPB Backend API`
   - Name: `gppb-backend-api`
   - API URL suffix: `gppb`
   - Products: `Default Product`

    ![API 관리자 - API 추가][image-06]

1. API 관리자에 API를 추가한 뒤 `GPPB Backend API` 👉 `GetWeatherForecast` 👉 `Test` 메뉴로 이동한 뒤, `Send` 버튼을 클릭합니다.

    ![API 관리자 - API 테스트][image-07]

1. 결과값이 잘 나오는 것을 확인합니다.

1. API 관리자에 방금 추가한 API를 다운로드 받습니다. `GPPB Backend API` 👉 `Export` 메뉴로 이동한 뒤, `OpenAPI v2 (JSON)` 버튼을 클릭하여 OpenAPI 문서를 다운로드 받습니다.

1. API 관리자 리소스의 왼쪽 메뉴에서 **Subscriptions 블레이드** 👉 `Built-in all-access subscription` 👉 `Primary key` 값을 복사합니다.

    ![API 관리자 - Subscriptions][image-08]

## 2. 파워 플랫폼 커스텀 커넥터 만들기

1. 파워 오토메이트에 접속합니다.

    ```text
    https://make.powerautomate.com
    ```

1. 두 개의 환경이 보입니다. 그 중 `Global Power Platform Bootcamp (default)` 대신 반드시 `Participant`로 시작하는 환경을 선택합니다.

1. 사용자 지정 커넥터 메뉴에서 `➕ 새 사용자 지정 커넥터` 👉 `OpenAPI 파일 가져오기` 메뉴를 선택합니다.

    ![사용자 지정 커넥터][image-09]

1. 커넥터 이름을 `GPPB`로 하고 앞서 다운로드 받은 OpenAPI 문서를 업로드합니다.

1. 설명 필드에 적당한 내용을 입력하고 `✔️ 커넥터 만들기` 버튼을 클릭해서 커스텀 커넥터를 만듭니다.

1. **테스트** 탭으로 이동하여 `➕ 새 연결` 버튼을 통해 연결을 만들고, `테스트 작업` 버튼을 클릭하여 잘 작동하는지 확인합니다.

## 3. 파워 오토메이트 워크플로우에서 커스텀 커넥터 사용하기

1. 왼쪽의 `➕ 만들기` 메뉴에서 `인스턴트 클라우드 흐름` 버튼을 클릭합니다.

1. 흐름 이름을 `GPPB Participant {{숫자}}`로 하고, `수동으로 흐름 트리거`를 선택한 후 `만들기` 버튼을 클릭합니다.

1. `➕` 버튼을 클릭하여 새 작업을 추가합니다.

1. 검색창에 `gppb`를 입력하고 방금 만든 커스텀 커넥터의 `GetWeatherForecast` 액션을 선택합니다.

1. `저장` 버튼을 클릭하여 파워 오토메이트 워크플로우를 저장합니다.

1. `테스트` 버튼을 클릭하여 잘 작동하는지 확인합니다. 만약 실패한다면, GitHub 코드스페이스의 백엔드 API 앱이 실행 중인지 확인합니다. 실행중이 아니라면, 다시 실행시킵니다.

1. 여전히 실패한다면 에러 메시지를 확인해 보세요. `date` 형식에 문제가 있을 겁니다. 다시 커스텀 커넥터 화면으로 돌아가서 `Swagger` 편집기를 실행시킵니다. 그리고 아래와 같이 수정합니다.

    ```yml
    # 수정 전 (line 75-79)
      WeatherForecast:
        type: object
        properties:
          date:
            $ref: '#/definitions/DateOnly'
    
    # 수정 전 (line 75-79)
      WeatherForecast:
        type: object
        properties:
          date:
            type: string
    ```

1. 다시 파워 오토메이트 워크플로우 편집 화면으로 들어가 앞서 추가했던 `GetWeatherForecast` 액션을 삭제하고 다시 추가합니다. 그리고 `테스트` 버튼을 클릭하여 잘 작동하는지 확인합니다.

---

축하합니다! API 관리자에 백엔드 API를 통합하고, 파워 오토메이트 화면에서 커스텀 커넥터를 만들어 봤습니다. 그리고 파워 오토메이트 워크플로우까지 만들어 봤습니다. 이제 [Session 04: 파워 플랫폼 CLI로 솔루션 익스포트하기](./04-power-platform-cli.md)로 넘어가세요.

[image-01]: ./images/03-image-01.png
[image-02]: ./images/03-image-02.png
[image-03]: ./images/03-image-03.png
[image-04]: ./images/03-image-04.png
[image-05]: ./images/03-image-05.png
[image-06]: ./images/03-image-06.png
[image-07]: ./images/03-image-07.png
[image-08]: ./images/03-image-08.png
[image-09]: ./images/03-image-09.png

[az apim]: https://learn.microsoft.com/ko-kr/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-87051-juyoo
