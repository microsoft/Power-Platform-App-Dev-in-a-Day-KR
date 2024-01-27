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

---

축하합니다! 커스텀 커넥터를 만들어 봤습니다. 이제 [Session 04: 파워 플랫폼 CLI로 솔루션 익스포트하기](./04-power-platform-cli.md)로 넘어가세요.

[az apim]: https://learn.microsoft.com/ko-kr/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-87051-juyoo
