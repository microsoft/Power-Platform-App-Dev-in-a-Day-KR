# 파워 플랫폼 커스텀 커넥터와 CI/CD 파이프라인 #

파워 플랫폼 커스텀 커넥터를 GitHub 리포지토리에서 관리하고 싶으신가요? GitHub Actions를 이용해 CI/CD 파이프라인을 구성해보세요. 이 트랙에서는 아래와 같은 내용을 다룹니다.

- [GitHub 코드스페이스][gh codespaces]를 사용해 백엔드 API를 개발합니다.
- [애저 Dev CLI][azd cli]를 이용해 애저에 리소스를 한번에 생성합니다.
- [애저 CLI][az cli]와 [GitHub 액션][gh actions]을 이용해 API를 애저에 배포합니다.
- API를 애저 백엔드 서비스인 [API 관리자][az apim]와 통합합니다.
- API 관리자를 통해 구성된 API를 [파워 플랫폼 커스텀 커넥터][pp cuscon]로 만듭니다.
- [파워 앱][pp apps]과 [파워 오토메이트][pp auto]에 커스텀 커넥터를 연동시켜 API를 호출합니다.
- [파워 플랫폼 CLI][pp cli]와 [GitHub 액션][gh actions]을 이용해 파워 앱, 파워 오토메이트, 커스텀 커넥터를 깃헙 리포지토리에 푸시합니다.

## 목차 ##

0. [개발 환경 설정하기](./00-setup.md)
1. [백엔드 API 개발하기](./01-api-app.md)
2. [애저 Dev CLI 이용해서 애저 인스턴스 만들기](./02-azd.md)
3. [애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./03-custom-connector.md)
4. [파워 플랫폼 CLI로 솔루션 익스포트하기](./04-power-platform-cli.md)
5. [GitHub 액션으로 파워 플랫폼 앱 관리하기](./05-github-actions.md)


[gh codespaces]: https://github.com/features/codespaces
[gh actions]: https://github.com/features/actions

[azd cli]: https://learn.microsoft.com/ko-kr/azure/developer/azure-developer-cli/overview?WT.mc_id=dotnet-87051-juyoo
[az cli]: https://learn.microsoft.com/ko-kr/cli/azure/what-is-azure-cli?WT.mc_id=dotnet-87051-juyoo

[az apim]: https://learn.microsoft.com/ko-kr/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-87051-juyoo

[pp apps]: https://learn.microsoft.com/ko-kr/power-apps/powerapps-overview?WT.mc_id=dotnet-87051-juyoo
[pp auto]: https://learn.microsoft.com/ko-kr/power-automate/getting-started?WT.mc_id=dotnet-87051-juyoo
[pp cuscon]: https://learn.microsoft.com/ko-kr/connectors/custom-connectors/?WT.mc_id=dotnet-87051-juyoo
[pp cli]: https://learn.microsoft.com/ko-kr/power-platform/developer/cli/introduction?WT.mc_id=dotnet-87051-juyoo
