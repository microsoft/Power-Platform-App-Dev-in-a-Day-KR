# Session 05: GitHub 액션으로 파워 플랫폼 앱 관리하기

앞서 터미널에서 [파워 플랫폼 CLI][pp cli]로 작업했던 파워 플랫폼 솔루션 내보내기/가져오기를 [GitHub 액션][gh actions]으로 자동화해 봅니다.

## 1. GitHub 액션 환경 변수 설정하기

1. 지난 세션에서 사용했던 변수 값들을 준비합니다.

   - 파워 플랫폼 환경 URL: `https://org{{조직 ID}}.crm{{숫자}}.dynamics.com/`
   - 애플리케이션(클라이언트) ID
   - 클라이언트 비밀
   - 디렉터리(테넌트) ID

1. 자신의 GitHub 리포지토리에서 `Settings` 👉 `Secrets and variables` 👉 `Actions` 메뉴로 이동합니다.

1. `New repository secret` 버튼을 클릭하고 아래와 같이 입력한 후, `Add secret` 버튼을 클릭합니다.

   - Name: `ENVIRONMENT_URL`
   - Secret: `https://org{{조직 ID}}.crm{{숫자}}.dynamics.com/`

1. 같은 식으로 반복해서 아래 내용도 입력합니다.

   - `APPLICATION_ID`
   - `CLIENT_SECRET`
   - `DIRECTORY_ID`

## 2. GitHub 액션 워크플로우 만들기 - 구조 설계

1. GitHub 액션 작업을 위해 아래 명령어를 통해 작업 디렉토리로 이동합니다.

    ```bash
    cd $CODESPACE_VSCODE_FOLDER
    ```

1. 아래와 같이 GitHub 액션 워크플로우를 위한 기본 구조를 만듭니다.

    ```bash
    mkdir -p .github/workflows
    touch .github/workflows/build-backend-api.yml
    touch .github/workflows/export-solution.yml
    touch .github/workflows/deploy-solution.yml
    ```

## 3. GitHub 액션 워크플로우 만들기 - 백엔드 API 빌드

1. `.github/workflows/build-backend-api.yml` 파일을 열고 아래와 같이 내용을 입력하여 기본 뼈대를 만듭니다.

    ```yml
    name: Build Backend API
    
    on:
      push:
        branches:
          - main
          - 'feature/*'
        paths:
          - 'devops-in-a-day/src/ApiApp/**'

    ```

1. 아래와 같이 내용을 입력하여 빌드 서버를 구성합니다. 여기서는 Ubuntu 리눅스를 사용합니다.

    ```yml
    jobs:
      build:
        runs-on: ubuntu-latest
    ```

1. 아래와 같이 내용을 입력하여 리포지토리의 최신 소스 코드를 가져옵니다.

    ```yml
        steps:
          - name: Checkout repository
            uses: actions/checkout@v4
    ```

1. 아래와 같이 내용을 입력하여 최신 .NET SDK를 설치합니다.

    ```yml
          - name: Setup .NET SDK
            uses: actions/setup-dotnet@v4
            with:
              dotnet-version: 8.x
    ```

1. 아래와 같이 내용을 입력하여 최신 백엔드 API 앱을 빌드하고 테스트합니다.

    ```yml
          - name: Restore NuGet packages
            shell: bash
            run: |
              dotnet restore ./devops-in-a-day/ApiApp.sln
    
          - name: Build
            shell: bash
            run: |
              dotnet build ./devops-in-a-day/ApiApp.sln --configuration Release
    
          - name: Run tests
            shell: bash
            run: |
              dotnet test ./devops-in-a-day/ApiApp.sln --configuration Release --no-build
    ```

1. `devops-in-a-day/src/ApiApp/Program.cs` 파일을 열어 맨 아래 빈 줄을 하나 추가한 후 커밋하여 푸시합니다.

1. GitHub 액션 워크플로우가 자동으로 실행되는 것을 확인합니다.

## 4. GitHub 액션 워크플로우 만들기 - 파워 플랫폼 솔루션 내보내기

1. `.github/workflows/export-solution.yml` 파일을 열고 아래와 같이 내용을 입력하여 기본 뼈대를 만듭니다.

    ```yml
    name: Export Power Platform Solution
    
    on:
      workflow_dispatch:
        inputs:
          solutionName:
            description: 'Solution Name'
            required: true
            default: 'GPPB'
          solutionPath:
            description: 'Solution Path'
            required: true
            default: 'devops-in-a-day/src/PowerPlatform'
          solutionType:
            description: 'Solution Type'
            required: true
            default: 'Unmanaged'
    ```

1. 아래와 같이 내용을 입력하여 환경 변수를 설정합니다.

    ```yml
    env:
      ENVIRONMENT_URL: ${{ secrets.ENVIRONMENT_URL }}
      APPLICATION_ID: ${{ secrets.APPLICATION_ID }}
      CLIENT_SECRET: ${{ secrets.CLIENT_SECRET }}
      DIRECTORY_ID: ${{ secrets.DIRECTORY_ID }}
    ```

1. 아래와 같이 내용을 입력하여 GitHub 액션의 권한을 설정합니다.

    ```yml
    permissions:
      contents: write
    ```

1. 아래와 같이 내용을 입력하여 빌드 서버를 구성합니다. 여기서는 Ubuntu 리눅스를 사용합니다.

    ```yml
    jobs:
      export_solution:
        runs-on: ubuntu-latest
    ```

1. 아래와 같이 내용을 입력하여 리포지토리의 최신 소스 코드를 가져옵니다.

    ```yml
        steps:
          - name: Checkout repository
            uses: actions/checkout@v4
    ```

1. 아래와 같이 내용을 입력하여 최신 파워 플랫폼 CLI를 설치합니다.

    ```yml
          - name: Install Power Platform Tools
            uses: microsoft/powerplatform-actions/actions-install@v1
    ```

1. 아래와 같이 내용을 입력하여 현재 접속한 파워 플랫폼 환경을 확인합니다.

    ```yml
          - name: Who am I?
            uses: microsoft/powerplatform-actions/who-am-i@v1
            with:
              environment-url: ${{ env.ENVIRONMENT_URL }}
              app-id: ${{ env.APPLICATION_ID }}
              client-secret: ${{ env.CLIENT_SECRET }}
              tenant-id: ${{ env.DIRECTORY_ID }}
    ```

1. 아래와 같이 내용을 입력하여 파워 플랫폼 솔루션을 내보내기 합니다.

    ```yml
          - name: Export solution
            uses: microsoft/powerplatform-actions/export-solution@v1
            with:
              environment-url: ${{ env.ENVIRONMENT_URL }}
              app-id: ${{ env.APPLICATION_ID }}
              client-secret: ${{ env.CLIENT_SECRET }}
              tenant-id: ${{ env.DIRECTORY_ID }}
              solution-name: ${{ github.event.inputs.solutionName }}
              solution-output-file: ${{ github.event.inputs.solutionPath}}/${{ github.event.inputs.solutionName }}.zip
    ```

1. 아래와 같이 내용을 입력하여 내보내기 한 솔루션의 압축을 해제합니다.

    ```yml
          - name: Unpack solution
            uses: microsoft/powerplatform-actions/unpack-solution@v1
            with:
              solution-file: ${{ github.event.inputs.solutionPath }}/${{ github.event.inputs.solutionName }}.zip
              solution-folder: ${{ github.event.inputs.solutionPath }}/${{ github.event.inputs.solutionName }}
              solution-type: ${{ github.event.inputs.solutionType }}
              overwrite-files: true
    ```

1. 아래와 같이 내용을 입력하여 내보내기한 솔루션을 새 브랜치로 만들어 커밋합니다.

    ```yml
          - name: Branch solution
            uses: microsoft/powerplatform-actions/branch-solution@v1
            with:
              solution-folder: ${{ github.event.inputs.solutionPath}}/${{ github.event.inputs.solutionName }}
              solution-target-folder: ${{ github.event.inputs.solutionPath}}/${{ github.event.inputs.solutionName }}
              repo-token: ${{ secrets.GITHUB_TOKEN }}
              allow-empty-commit: true
    ```

1. 리포지토리의 `Actions` 탭을 클릭하여 `Export Power Platform Solution` 👉 `Run workflow` 메뉴를 선택하고 아래 내용을 입력한 후 `Run workflow` 버튼을 클릭합니다.

   - Solution Name: `GPPBParticipant{{숫자}}`
   - Solution Path: `devops-in-a-day/src/PowerPlatform`
   - Solution Type: `Unmanaged`

1. GitHub 액션 워크플로우가 실행되는 것을 확인합니다.

1. GitHub 액션 워크플로우 종료 이후 Pull Request를 생성합니다.

    ![Pull Request #1][image-01]

   > **NOTE**: 이 때 자신의 리포지토리로 Pull Request 생성하는 것을 잊지 마세요!
   >
   > ![Pull Request #2][image-02]

   Pull Request를 생성합니다.

    ![Pull Request #3][image-03]

1. Pull Request를 병합합니다.

    ![Pull Request #4][image-04]

1. 병합 후 `main` 브랜치에서 병합 내용을 확인합니다.

## 5. GitHub 액션 워크플로우 만들기 - 파워 플랫폼 솔루션 배포하기

1. 먼저 환경에 이미 있는 솔루션을 삭제합니다.

    ```bash
    pac solution delete --solution-name GPPBParticipant{{숫자}}
    ```

1. 파워 오토메이트 웹사이트에서 솔루션이 사라진 것을 확인합니다.

1. `.github/workflows/deploy-solution.yml` 파일을 열고 아래와 같이 내용을 입력하여 기본 뼈대를 만듭니다.

    ```yml
    name: Deploy Power Platform Solution
    
    on:
      workflow_dispatch:
        inputs:
          solutionName:
            description: 'Solution Name'
            required: true
            default: 'GPPB'
          solutionPath:
            description: 'Solution Path'
            required: true
            default: 'devops-in-a-day/src/PowerPlatform'
          solutionType:
            description: 'Solution Type'
            required: true
            default: 'Unmanaged'
    ```

1. 아래와 같이 내용을 입력하여 환경 변수를 설정합니다.

    ```yml
    env:
      ENVIRONMENT_URL: ${{ secrets.ENVIRONMENT_URL }}
      APPLICATION_ID: ${{ secrets.APPLICATION_ID }}
      CLIENT_SECRET: ${{ secrets.CLIENT_SECRET }}
      DIRECTORY_ID: ${{ secrets.DIRECTORY_ID }}
    ```

1. 아래와 같이 내용을 입력하여 빌드 서버를 구성합니다. 여기서는 Ubuntu 리눅스를 사용합니다.

    ```yml
    jobs:
      deploy_solution:
        runs-on: ubuntu-latest
    ```

1. 아래와 같이 내용을 입력하여 리포지토리의 최신 소스 코드를 가져옵니다.

    ```yml
        steps:
          - name: Checkout repository
            uses: actions/checkout@v4
    ```

1. 아래와 같이 내용을 입력하여 최신 파워 플랫폼 CLI를 설치합니다.

    ```yml
          - name: Install Power Platform Tools
            uses: microsoft/powerplatform-actions/actions-install@v1
    ```

1. 아래와 같이 내용을 입력하여 현재 접속한 파워 플랫폼 환경을 확인합니다.

    ```yml
          - name: Who am I?
            uses: microsoft/powerplatform-actions/who-am-i@v1
            with:
              environment-url: ${{ env.ENVIRONMENT_URL }}
              app-id: ${{ env.APPLICATION_ID }}
              client-secret: ${{ env.CLIENT_SECRET }}
              tenant-id: ${{ env.DIRECTORY_ID }}
    ```

1. 아래와 같이 내용을 입력하여 솔루션을 zip 파일로 압축합니다.

    ```yml
          - name: Pack solution
            uses: microsoft/powerplatform-actions/pack-solution@v1
            with:
              solution-file: ${{ github.event.inputs.solutionPath }}/${{ github.event.inputs.solutionName }}.zip
              solution-folder: ${{ github.event.inputs.solutionPath }}/${{ github.event.inputs.solutionName }}
              solution-type: ${{ github.event.inputs.solutionType }}
    ```

1. 아래와 같이 내용을 입력하여 파워 플랫폼 솔루션을 배포합니다.

    ```yml
          - name: Deploy solution
            uses: microsoft/powerplatform-actions/import-solution@v1
            with:
              environment-url: ${{ env.ENVIRONMENT_URL }}
              app-id: ${{ env.APPLICATION_ID }}
              client-secret: ${{ env.CLIENT_SECRET }}
              tenant-id: ${{ env.DIRECTORY_ID }}
              solution-file: ${{ github.event.inputs.solutionPath }}/${{ github.event.inputs.solutionName }}.zip
              force-overwrite: true
    ```

1. 리포지토리의 `Actions` 탭을 클릭하여 `Deploy Power Platform Solution` 👉 `Run workflow` 메뉴를 선택하고 아래 내용을 입력한 후 `Run workflow` 버튼을 클릭합니다.

   - Solution Name: `GPPBParticipant{{숫자}}`
   - Solution Path: `devops-in-a-day/src/PowerPlatform`
   - Solution Type: `Unmanaged`

1. GitHub 액션 워크플로우가 실행되는 것을 확인합니다.

1. 파워 오토메이트 웹사이트에서 솔루션이 다시 들어온 것을 확인하고 그 안으로 들어갑니다.

1. 클라우드 흐름을 선택하고 편집 모드로 들어갑니다.

1. 흐름을 저장한 후 테스트를 실행해서 워크플로우가 잘 작동하는 것을 확인합니다.

---

축하합니다! GitHub 액션 워크플로우를 이용해서 파워 플랫폼 솔루션을 내보내기 하고 배포하기를 해 봤습니다.

> **NOTE**: 만약 GitHub 액션 워크플로우 작성을 마치지 못했다면 [여기](./.github/workflows/)에서 코드를 확인할 수 있습니다.

[image-01]: ./images/05-image-01.png
[image-02]: ./images/05-image-02.png
[image-03]: ./images/05-image-03.png
[image-04]: ./images/05-image-04.png

[gh actions]: https://github.com/features/actions

[pp cli]: https://learn.microsoft.com/ko-kr/power-platform/developer/cli/introduction?WT.mc_id=dotnet-87051-juyoo
