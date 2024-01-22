# Session 01: 애저 Dev CLI 이용해서 애저 인스턴스 만들기 #

백엔드 API에 연결된 커스텀 커넥터를 만들기 위해서는 애저에 리소스를 생성해야 합니다. 아래 순서대로 따라해 보세요.

- [Session 01: 애저 Dev CLI 이용해서 애저 인스턴스 만들기](#session-01-애저-dev-cli-이용해서-애저-인스턴스-만들기)
  - [1. 애저에 리소스 생성하기](#1-애저에-리소스-생성하기)
  - [2. 애저와 GitHub 연동시키기](#2-애저와-github-연동시키기)


## 1. 애저에 리소스 생성하기 ##

1. 아래와 같이 랜덤 숫자를 생성합니다. 랜덤 숫자 자릿수가 4자리, 5자리, 6자리 등 다양하게 나오는데, 5자리까지만 끊어주세요.

    ```bash
    echo $RANDOM
    ```

1. [애저 Dev CLI][azd cli]를 이용해서 애저에 로그인합니다. 아래 명령어를 입력하세요.

    ```bash
    azd login
    ```

1. 아래 명령어를 이용해 `azd` 환경 설정을 시작합니다.

    ```bash
    azd init
    ```

1. 프롬프트에 따라 아래와 같이 입력합니다.

   - Select a project template: `Empty Template`
   - Please enter a new environment name: `gppb{{랜덤숫자}}` ⬅️ `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.
   - Please select an Azure Subscription to use: 구독 이름 선택
   - Please select an Azure location to use: `(Asia Pacific) Korea Central (koreacentral)`

1. 아래 파일이 만들어진 것을 확인합니다.

   - `azure.yaml`
   - `.azure/gppb{{랜덤숫자}}/.env`

1. `azure.yaml` 파일을 열고 아래와 같이 수정합니다.

    ```yaml
    # yaml-language-server: $schema=https://raw.githubusercontent.com/Azure/azure-dev/main/schemas/v1.0/azure.yaml.json

    name: Power-Platform-App-Dev-in-a-Day-KR

    infra:
      provider: "bicep"
      path: "devops-in-a-day/infra"
      module: "main"

    pipeline:
      provider: "github"
    ```

1. 아래 명령어를 실행시켜 애저에 리소스를 생성합니다.

    ```bash
    azd up
    ```

1. [애저 포털][az portal]에서 생성된 리소스를 확인합니다.

    ![리소스 프로비저닝 결과][image01]

## 2. 애저와 GitHub 연동시키기 ##

1. [GitHub 코드스페이스][gh codespaces]에서 애저와 GitHub을 연동시키기 위해서는 아래와 같이 [애저 CLI][az cli] 명령어를 통해 다시 애저에 로그인합니다.

    ```bash
    az login
    ```

1. 로그인 후 현재 설정되어 있는 구독을 확인합니다.

    ```bash
    az account show
    ```

1. 만약 현재 설정된 구독과 다른 구독을 사용하고 싶다면 아래 명령어를 통해 현재 로그인한 계정에 물려있는 구독 리스트를 확인합니다.

    ```bash
    az account list --query "[].name" -o tsv
    ```

1. 위의 리스트에서 내가 원하는 구독 이름이 `azure-gppb` 이라고 가정합니다. 그러면 아래 명령어를 통해 내가 원하는 구독으로 설정합니다.

    ```bash
    az account set --subscription azure-gppb
    ```

1. 다시 아래 명령어를 통해 내가 원하는 구독으로 바뀌었는지 확인합니다.

    ```bash
    az account show
    ```

---

여기까지 해서 애저 Dev CLI와 Azure CLI를 이용해서 애저와 깃헙, 그리고 코드스페이스 연동을 마쳤습니다.

- [애저 Dev CLI 더 알아보기][azd cli]
- [애저 CLI 더 알아보기][az cli]

---

- 다음 세션: [API Key 인증 API 개발, 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./2-custom-connector.md)


[image01]: ./images/01-image01.png


[az portal]: https://portal.azure.com?WT.mc_id=dotnet-87051-juyoo

[azd cli]: https://learn.microsoft.com/ko-kr/azure/developer/azure-developer-cli/overview?WT.mc_id=dotnet-87051-juyoo
[az cli]: https://learn.microsoft.com/ko-kr/cli/azure/what-is-azure-cli?WT.mc_id=dotnet-87051-juyoo

[gh codespaces]: https://github.com/features/codespaces
