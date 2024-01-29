# Session 04: 파워 플랫폼 CLI로 솔루션 내보내기

앞서 만든 파워 오토메이트 워크플로우와 커스텀 커넥터를 깃헙 리포지토리에 푸시하기 위하여 [파워 플랫폼 CLI][pp cli]를 사용해 보겠습니다.

## 1. 솔루션 생성하기

1. 파워 오토메이트에 접속합니다.

    ```text
    https://make.powerautomate.com
    ```

1. 두 개의 환경이 보입니다. 그 중 `Global Power Platform Bootcamp (default)` 대신 반드시 `Participant`로 시작하는 환경을 선택합니다.

1. 왼쪽의 `솔루션` 메뉴에서 `➕ 새 솔루션` 버튼을 클릭합니다.

   > **NOTE**: 만약 `솔루션` 메뉴가 보이지 않는다면, 왼쪽의 `더 보기` 메뉴에서 `솔루션`을 선택합니다.

1. 새 솔루션 만들기 창이 나타나면 아래 내용을 입력합니다.

   - 표시 이름: `GPPB Participant {{숫자}}`
   - 이름: `GPPBParticipant{{숫자}}`
   - 게시자:
     - `➕ 새 게시자` 선택
     - 표시 이름: `Participant {{숫자}}`
     - 이름: `participant{{숫자}}`
     - 접두사: `pp{{숫자}}`
     - 선택 값 접두사: 기본값
     - `저장` 버튼 클릭
     - 방금 생성한 `Participant {{숫자}}` 게시자 선택
   - `만들기` 버튼 클릭

1. 상단의 `기존 항목 추가` 버튼 👉 `자동화` 👉 `사용자 지정 커넥터` 메뉴를 선택합니다.

1. `Dataverse 외부` 탭에서 `GPPB`를 선택한 후 `추가` 버튼을 클릭합니다.

1. 상단의 `기존 항목 추가` 버튼 👉 `자동화` 👉 `클라우드 흐름` 메뉴를 선택합니다.

1. `Dataverse 외부` 탭에서 `GPPB Participant {{숫자}}`를 선택한 후 `추가` 버튼을 클릭합니다.

1. 아래와 같이 솔루션에 커스텀 커넥터와 파워 오토메이트 워크플로우가 추가된 것을 확인합니다.

   ![솔루션][image-01]

1. `GPPB Participant {{숫자}}`클라우드 흐름을 선택하고 편집 모드로 들어갑니다.

1. `GetWeatherForecast` 액션이 `기타 연결 참조`를 사용하고 있는지 확인합니다.

   ![기타 연결 참조][image-02]

1. `저장` 버튼을 클릭한 후 `테스트` 버튼을 클릭하여 잘 작동하는지 확인합니다.

## 2. 서비스 어카운트 만들기

1. 애저 포털에 접속합니다. 접속시 본인의 `participant_{{숫자}}@gppbkr.onmicrosoft.com` 계정을 사용합니다.

   ```text
   https://portal.azure.com
   ```

1. `Microsoft Entra ID` 서비스에 접속합니다.

1. `앱 등록` 블레이드 👉 `➕ 새 등록` 메뉴를 클릭합니다.

1. 애플리케이션 등록 화면에서 아래 내용을 입력합니다.

   - 이름: `GPPB Participant {{숫자}}`
   - 지원되는 계정 유형: `이 조직 디렉터리의 계정만(Global Power Platform Bootcamp만 - 단일 테넌트)`
   - 리디렉션 URI: `웹` 👉 `https://localhost`
   - `등록` 버튼 클릭

1. 만들어진 앱에서 `애플리케이션(클라이언트) ID`, `디렉터리(테넌트) ID` 값을 복사해 둡니다.

1. 왼쪽의 `인증서 및 암호` 블레이드를 선택한 후 `클라이언트 비밀` 탭에서 `➕ 새 클라이언트 암호` 메뉴를 클릭합니다.

1. 아래 내용을 입력한 후 `추가` 버튼을 클릭합니다.

   - 설명: `gppb`
   - 만료시간: `권장 180일 (6개월)`

1. 만들어진 클라이언트 비밀에서 값을 복사해 둡니다.

   > **NOTE**:
   > 
   > - 이 화면을 벗어나면 다시는 이 값을 확인할 수 없으니 꼭 복사해 두세요.
   > - 만약 시크릿 값이 `-`로 시작한다면 다시 만드세요.

1. 왼쪽의 `API 사용 권한` 블레이드를 선택합니다.

1. `➕ 권한 추가` 버튼을 클릭한 후 `Microsoft API` 탭에서 `Dynamics CRM`을 선택합니다. 이후 `위임된 권한` 메뉴를 선택하고 `user_impersonation` 항목을 체크한 후 `권한 추가` 버튼을 클릭합니다.

1. `➕ 권한 추가` 버튼을 클릭한 후 `Microsoft API` 탭에서 `Power Automate`를 선택합니다. 이후 `위임된 권한` 메뉴를 선택하고 `User` 항목을 체크한 후 `권한 추가` 버튼을 클릭합니다.

1. `➕ 권한 추가` 버튼을 클릭한 후 `Microsoft API` 탭에서 `PowerApps Runtime Service`를 선택합니다. 이후 `위임된 권한` 메뉴를 선택하고 `user_impersonation` 항목을 체크한 후 `권한 추가` 버튼을 클릭합니다.

1. `➕ 권한 추가` 버튼을 클릭한 후 `내 조직에서 사용하는 API` 탭에서  `PowerApps-Advisor`를 선택합니다. 이후 `위임된 권한` 메뉴를 선택하고 `Analysis.All` 항목을 체크한 후 `권한 추가` 버튼을 클릭합니다.

1. `✔️ Global Power Platform Bootcamp에 대한 관리자 동의 허용` 메뉴를 클릭하고 `예` 버튼을 클릭합니다.

## 3. 파워 플랫폼 로그인

1. 자신의 환경에 해당하는 URL을 찾습니다.

   - 파워 오토메이트 화면의 주소를 복사합니다. 주소는 아래와 같은 형식입니다. 자신의 `Participant` 환경과 `Global Power Platform Bootcamp (default)` 환경의 환경 ID를 찾습니다.

     ```text
     https://make.powerautomate.com/environments/{{환경 ID}}/solutions
     ```

   - 환경 ID 값을 바탕으로 아래 관리자 URL로 접속합니다.

     ```text
     https://admin.powerplatform.microsoft.com/environments/environment/{{환경 ID}}/hub
     ```

   - 환경 URL 값을 복사합니다. URL은 아래와 같은 형식입니다.

     ```text
     https://org{{조직 ID}}.crm{{숫자}}.dynamics.com/
     ```

1. 상단의 `⚙️ 설정` 버튼을 클릭한 후 `사용자 + 권한` 👉 `응용 프로그램 사용자` 메뉴를 선택합니다.

1. `➕ 새 앱 사용자` 버튼을 클릭하여 아래 내용을 입력하고 `만들기` 버튼을 클릭합니다.

   - 앱: `GPPB Participant {{숫자}}`
   - 사업부: 기본값
   - 보안 역할: `시스템 관리자`

1. 새 터미널을 열고 아래 명령어를 실행하여 파워 플랫폼 CLI 설치 여부를 확인합니다.

    ```bash
    pac help
    ```

1. 아래 명령어를 통해 파워 플랫폼 로그인 여부를 확인합니다. `No profiles were found on this computer. Please run 'pac auth create' to create one.` 메시지가 보이면 정상입니다.

    ```bash
    pac auth list
    ```

1. 아래 명령어를 통해 파워 플랫폼에 로그인합니다. URL 값은 앞서 복사한 환경 URL을 입력합니다. 환경 URL은 `https://org{{조직 ID}}.crm{{숫자}}.dynamics.com/`와 같은 형식입니다.

    ```bash
    pac auth create --name PARTICIPANT --url {{Participant 환경 URL}} --applicationId {{애플리케이션 ID}} --clientSecret {{클라이언트 비밀}} --tenant {{디렉터리 ID}}
    ```

1. 같은 방식으로 이번에는 아래 명령어를 통해 파워 플랫폼의 기본 환경에 로그인합니다.

    ```bash
    pac auth create --name DEFAULT --url {{Default 환경 URL}} --applicationId {{애플리케이션 ID}} --clientSecret {{클라이언트 비밀}} --tenant {{디렉터리 ID}}
    ```

1. 이후 아래 명령어를 통해 현재 로그인된 환경을 확인합니다.

    ```bash
    pac auth list
    ```

   > **NOTE**: 만약 `DEFAULT` 환경이 선택되어 있으면 아래 명령어를 통해 `PARTICIPANT` 환경으로 변경합니다.
   > 
   > ```bash
   > pac auth list
   > pac auth select --name PARTICIPANT
   > ```

## 4. 솔루션 내보내기

1. GitHub 리포지토리에 솔루션을 내보내기 위해 아래 명령어를 통해 작업 디렉토리로 이동합니다.

   ```bash
   cd $CODESPACE_VSCODE_FOLDER/devops-in-a-day
   ```

1. 아래 명령어를 통해 현재 로그인된 환경의 솔루션을 확인합니다. `GPPB Participant {{숫자}}` 솔루션이 보이면 정상입니다.

    ```bash
    pac solution list
    ```

1. 아래 명령어를 통해 솔루션을 내보냅니다.

    ```bash
    pac solution export --name GPPBParticipant{{숫자}} --path ./src/PowerPlatform --overwrite
    ```

   > **NOTE**: `--overwrite` 파라미터는 이미 내보내진 솔루션을 덮어쓰기 위한 옵션입니다.

1. 솔루션이 내보내진 디렉토리로 이동한 뒤 내용을 확인합니다.

    ```bash
    cd ./src/PowerPlatform
    ls -al
    ```

1. 솔루션 파일의 압축을 해제합니다.

    ```bash
    unzip ./GPPBParticipant{{숫자}}.zip -d ./GPPBParticipant{{숫자}}
    ```

1. 압축을 해제한 뒤 디렉토리 구조를 확인합니다.

    ![디렉토리 구조][image-03]

1. `Connector` 폴더 아래의 `*_openapidefinition.json` 파일을 열어 OpenAPI 문서를 확인합니다.

1. `Workflows` 폴더 아래의 `*.json` 파일을 열어 워크플로우를 확인합니다.

1. 아래 명령어를 통해 솔루션 파일을 GitHub 리포지토리에 푸시합니다.

    ```bash
    git switch -c feature/gppb-participant{{숫자}}
    git add .
    git commit -m "Add Power Platform solution"
    git push origin
    ```

1. 파워 플랫폼 CLI를 통해 다운로드 받은 솔루션 파일을 삭제합니다.

    ```bash
    rm -rf ./GPPBParticipant{{숫자}}.zip
    ```

## 5. 솔루션 가져오기

1. 먼저 환경에 이미 있는 솔루션을 삭제합니다.

    ```bash
    pac solution delete --solution-name GPPBParticipant{{숫자}}
    ```

1. 파워 오토메이트 웹사이트에서 솔루션이 사라진 것을 확인합니다.

1. GitHub 리포지토리에 푸시한 솔루션을 가져오기 위해 아래 명령어를 통해 작업 디렉토리로 이동합니다.

    ```bash
    cd $CODESPACE_VSCODE_FOLDER/devops-in-a-day
    ```

1. 솔루션이 있는 디렉토리로 이동합니다.

    ```bash
    cd ./src/PowerPlatform
    ls -al
    ```

1. 아래 명령어를 통해 솔루션 데이터를 압축합니다.

    ```bash
    pushd ./GPPBParticipant01/
    zip -r ./GPPBParticipant{{숫자}}.zip ./GPPBParticipant{{숫자}}
    mv ./GPPBParticipant01.zip ../
    popd
    ```

1. 아래 명령어를 통해 솔루션을 가져옵니다.

    ```bash
    pac solution import --path ./GPPBParticipant{{숫자}}.zip --force-overwrite
    ```

   > **NOTE**: `--force-overwrite` 파라미터는 이미 가져온 솔루션을 덮어쓰기 위한 옵션입니다.

1. 파워 오토메이트 웹사이트에서 솔루션이 다시 들어온 것을 확인하고 그 안으로 들어갑니다.

1. 클라우드 흐름을 선택하고 편집 모드로 들어갑니다.

1. 흐름을 저장한 후 테스트를 실행해서 워크플로우가 잘 작동하는 것을 확인합니다.

---

축하합니다! 파워 플랫폼 CLI로 솔루션 내보내기와 가져오기를 해 봤습니다. 이제 [Session 05: GitHub 액션으로 파워 플랫폼 앱 관리하기](./05-github-actions.md)로 넘어가세요.

[image-01]: ./images/04-image-01.png
[image-02]: ./images/04-image-02.png
[image-03]: ./images/04-image-03.png

[pp cli]: https://learn.microsoft.com/ko-kr/power-platform/developer/cli/introduction?WT.mc_id=dotnet-87051-juyoo
