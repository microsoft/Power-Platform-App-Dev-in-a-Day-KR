# Azure Dev CLI 이용 애저 인스턴스 만들기 #

애저에 API 앱을 배포하기 위해서는 우선 애저에 API 앱 인스턴스를 생성해야 합니다. 아래 순서대로 따라해 보세요.

1. 아래와 같이 랜덤 숫자를 생성합니다.

    ```bash
    echo $RANDOM
    ```

2. 애저에 로그인합니다. 아래 명령어를 입력하세요.

    ```bash
    azd login
    ```

3. 아래 명령어를 이용해 `azd` 환경 설정을 시작합니다.

    ```bash
    azd init
    ```

4. 프롬프트에 따라 아래와 같이 입력합니다.

   * Select a project template: `Empty Template`
   * Please enter a new environment name: `gppb{{랜덤숫자}}` ⬅️ `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다.
   * Please select an Azure Subscription to use: 구독 이름 선택
   * Please select an Azure location to use: `(Asia Pacific) Korea Central (koreacentral)`

5. 아래 파일이 만들어진 것을 확인합니다.

   * `azure.yaml`
   * `.azure/gppb{{랜덤숫자}}/.env`

6. `azure.yaml` 파일을 열고 아래와 같이 수정합니다.

    ```yaml
    # yaml-language-server: $schema=https://raw.githubusercontent.com/Azure/azure-dev/main/schemas/v1.0/azure.yaml.json

    name: Power-Platform-App-Dev-in-a-Day-KR

    infra:
    provider: "bicep"
    path: "custom-connectors-in-a-day/infra"
    module: "main"

    pipeline:
    provider: "github"
    ```

7. `.azure/gppb{{랜덤숫자}}/.env` 파일을 열고 파일의 맨 밑에 아래 내용을 추가합니다. `{{GitHub ID}}`는 여러분의 GitHub ID입니다.

    ```text
    GITHUB_USERNAME="{{GitHub ID}}"
    GITHUB_REPOSITORY_NAME="Power-Platform-App-Dev-in-a-Day-KR"
    ```

8. 아래 명령어를 실행시켜 애저에 리소스를 생성합니다.

    ```bash
    azd up
    ```

9. 애저 포털에서 생성된 리소스를 확인합니다.

    ![리소스 프로비저닝 결과][image01]


[image01]: ./images/session01-image01.png
