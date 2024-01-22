# Session 02: 애저 Dev CLI 이용해서 애저 인스턴스 만들기 #

백엔드 API에 연결된 커스텀 커넥터를 만들기 위해서는 애저에 리소스를 생성해야 합니다. 아래 순서대로 따라해 보세요.

## 1. 애저에 리소스 생성하기 ##

1. 아래 명령어를 통해 작업 디렉토리로 이동합니다. 이곳이 이번 세션에서 사용할 디렉토리입니다.

   ```bash
   cd $CODESPACE_VSCODE_FOLDER/devops-in-a-day
   ```

1. 아래와 같이 랜덤 숫자를 생성합니다. 랜덤 숫자 자릿수가 4자리, 5자리, 6자리 등 다양하게 나옵니다.

    ```bash
    AZURE_ENV_NAME="gppb$RANDOM"
    ```

1. 앞서 이미 [애저 Dev CLI][azd cli]를 통해 애저에 로그인했습니다. 아래 명령어를 이용해 `azd` 환경 설정을 시작합니다.

    ```bash
    azd init -e $AZURE_ENV_NAME
    ```

1. 프롬프트에 따라 아래와 같이 입력합니다.

   - How do you want to initialize your app: `Select a template`
   - Continue initializing an app in '/workspaces/Power-Platform-App-Dev-in-a-Day-KR/devops-in-a-day: 'Y'
   - Select a project template: `Minimal`
   - What would you like to do with these files: `Overwrite with versions from template`

1. 아래 파일이 만들어진 것을 확인합니다.

   - `azure.yaml`
   - `.azure/gppb{{랜덤숫자}}/.env`

1. `azure.yaml` 파일을 열고 아래와 같이 수정합니다.

    ```yaml
    # yaml-language-server: $schema=https://raw.githubusercontent.com/Azure/azure-dev/main/schemas/v1.0/azure.yaml.json

    name: Power-Platform-App-Dev-in-a-Day-KR

    infra:
      provider: "bicep"
      path: "infra"
      module: "main"

    pipeline:
      provider: "github"
    ```

1. 아래 명령어를 실행시켜 애저에 리소스를 생성합니다.

    ```bash
    azd provision
    ```

1. 프롬프트에 따라 아래와 같이 입력합니다.

   - Select an Azure Subscription to use: 구독 이름 선택
   - Select an Azure location to use: `(Asia Pacific) Korea Central (koreacentral)`

1. [애저 포털][az portal]에서 생성된 리소스를 확인합니다.

    ![리소스 프로비저닝 결과][image01]

---

축하합니다! 애저 Dev CLI를 이용해서 애저에 앱 배포를 위핸 인스턴스 생성을 마쳤습니다. 이제 [Session 03: 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./03-custom-connector.md)로 넘어가세요.

[image01]: ./images/01-image01.png

[az portal]: https://portal.azure.com?WT.mc_id=dotnet-87051-juyoo

[azd cli]: https://learn.microsoft.com/ko-kr/azure/developer/azure-developer-cli/overview?WT.mc_id=dotnet-87051-juyoo
