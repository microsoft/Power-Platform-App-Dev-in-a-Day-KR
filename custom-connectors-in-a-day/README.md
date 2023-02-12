# 파워 플랫폼 커스텀 커넥터와 애저 백엔드 통합하기 #

여러분 회사 내부에서만 사용하는 API를 파워 플랫폼에서 사용하고 싶으신가요? 그럴 때 커스텀 커넥터를 활용할 수 있습니다. 이 트랙에서는 아래와 같은 내용을 다룹니다.

- [GitHub 코드스페이스][gh codespaces]를 사용해 백엔드 API를 개발합니다.
- [애저 Dev CLI][azd cli]를 이용해 애저에 리소스를 한번에 생성합니다.
- [애저 CLI][az cli]와 [GitHub 액션][gh actions]을 이용해 API를 애저에 배포합니다.
- API를 애저 백엔드 서비스인 [API 관리자][az apim]와 통합합니다.
- API 관리자를 통해 구성된 API를 [파워 플랫폼 커스텀 커넥터][pp cuscon]로 만듭니다.
- [파워 앱][pp apps]과 [파워 오토메이트][pp auto]에 커스텀 커넥터를 연동시켜 API를 호출합니다.


## 목차 ##

1. [애저 Dev CLI 이용해서 애저 인스턴스 만들기](./1-azd.md)
2. [API Key 인증 API 개발, 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./2-api-key-auth.md)
3. [Basic 인증 API 개발, 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./3-basic-auth.md)
4. [OAuth2 인증 API 개발, 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./4-oauth2-auth.md)


[gh signup]: https://github.com/signup
[gh codespaces]: https://github.com/features/codespaces
[gh actions]: https://github.com/features/actions

[azd cli]: https://learn.microsoft.com/ko-kr/azure/developer/azure-developer-cli/overview?WT.mc_id=dotnet-87051-juyoo
[az cli]: https://learn.microsoft.com/ko-kr/cli/azure/what-is-azure-cli?WT.mc_id=dotnet-87051-juyoo

[az free]: https://azure.microsoft.com/ko-kr/free/?WT.mc_id=dotnet-87051-juyoo
[az apim]: https://learn.microsoft.com/ko-kr/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-87051-juyoo

[m365 dev]: https://developer.microsoft.com/microsoft-365/dev-program?WT.mc_id=dotnet-87051-juyoo

[pp dev]: https://powerapps.microsoft.com/developerplan/?WT.mc_id=dotnet-87051-juyoo
[pp apps]: https://learn.microsoft.com/ko-kr/power-apps/powerapps-overview?WT.mc_id=dotnet-87051-juyoo
[pp auto]: https://learn.microsoft.com/ko-kr/power-automate/getting-started?WT.mc_id=dotnet-87051-juyoo
[pp cuscon]: https://learn.microsoft.com/ko-kr/connectors/custom-connectors/?WT.mc_id=dotnet-87051-juyoo

[docker desktop]: https://docs.docker.com/desktop/

[wsl]: https://learn.microsoft.com/ko-kr/windows/wsl/install?WT.mc_id=dotnet-87051-juyoo

[vs code]: https://code.visualstudio.com/?WT.mc_id=dotnet-87051-juyoo
[vs code extensions remote]: https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.vscode-remote-extensionpack&WT.mc_id=dotnet-87051-juyoo
