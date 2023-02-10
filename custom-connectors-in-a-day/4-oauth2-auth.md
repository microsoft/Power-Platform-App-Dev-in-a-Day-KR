# OAuth2 인증 API 활용 커스텀 커넥터 만들기 #

파워 플랫폼 커스텀 커넥터에 연동시키기 위한 API를 개발합니다. 이 API는 애저 API 관리자에서 프록시 형태로 지원하는 OAuth2 인증을 구현합니다. 아래 순서대로 따라해 보세요.

- [OAuth2 인증 API 활용 커스텀 커넥터 만들기](#oauth2-인증-api-활용-커스텀-커넥터-만들기)
  - [1. OAuth2 인증용 앱 등록하기](#1-oauth2-인증용-앱-등록하기)
  - [2. API 앱 개발하기](#2-api-앱-개발하기)
  - [3. GitHub 액션 연동후 자동 배포하기](#3-github-액션-연동후-자동-배포하기)
  - [4. API 관리자 연동하기](#4-api-관리자-연동하기)
  - [5. API 관리자 OAuth2 등록하기](#5-api-관리자-oauth2-등록하기)
  - [6. 파워 플랫폼 커스텀 커넥터 생성하기](#6-파워-플랫폼-커스텀-커넥터-생성하기)
  - [7. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기](#7-파워-앱과-파워-오토메이트에서-커스텀-커넥터-사용하기)
    - [파워 오토메이트](#파워-오토메이트)
    - [파워 앱](#파워-앱)

## 1. OAuth2 인증용 앱 등록하기 ##

이번에 사용하려는 API는 OAuth2 인증을 통한 액세스 토큰이 필요합니다. 이 액세스 토큰을 발급받기 위해서는 먼저 [OAuth2 인증용 앱][az ad oauth2]을 [애저 액티브 디렉토리][az ad]에 등록해야 합니다. 아래 순서대로 따라해 보세요.

1. 애저 포털 상단의 검색창에 `azure active directory`를 검색합니다.

    ![애저 포털에서 액티브 디렉토리 검색하기][image01]

2. 액티브 디렉토리 화면에서 **앱 등록** 블레이드로 이동합니다. 그러면 이미 [애저 Dev CLI 이용해서 애저 인스턴스 만들기](./1-azd.md) 세션에서 애저 CLI를 통해 **spn-gppb{{랜덤숫자}}**라는 이름으로 만들었던 앱이 하나 있는 것이 보입니다. `{{랜덤숫자}}`는 앞서 `echo $RANDOM`으로 생성한 숫자를 가리킵니다. 이 앱을 클릭해서 들어갑니다.

    ![등록한 앱 확인][image02]


## 2. API 앱 개발하기 ##

TBD


## 3. GitHub 액션 연동후 자동 배포하기 ##

TBD


## 4. API 관리자 연동하기 ##

TBD


## 5. API 관리자 OAuth2 등록하기 ##

TBD


## 6. 파워 플랫폼 커스텀 커넥터 생성하기 ##

TBD


## 7. 파워 앱과 파워 오토메이트에서 커스텀 커넥터 사용하기 ##

TBD


### 파워 오토메이트 ###

TBD


### 파워 앱 ###

TBD



파워 앱에서 파워 오토메이트를 통해 커스텀 커넥터를 연결하고 API를 호출해 봤습니다.

---

여기까지 해서 Basic 인증을 통한 파워 플랫폼 커스텀 커넥터를 만들고, 이를 파워 앱과 파워 오토메이트에서 활용해 봤습니다.

- [애저 액티브 디렉토리 OAuth2 &ndash; 인증 코드 플로우][az ad oauth2 flow authcode]
- [파워 앱 더 알아보기][pp apps]
- [파워 오토메이트 더 알아보기][pp auto]

---

- 이전 세션: [Basic 인증 API 개발, 애저 API 관리자와 통합, 그리고 커스텀 커넥터 만들기](./3-basic-auth.md)


[image01]: ./images/session04-image01.png
[image02]: ./images/session04-image02.png
[image03]: ./images/session04-image03.png
[image04]: ./images/session04-image04.png
[image05]: ./images/session04-image05.png
[image06]: ./images/session04-image06.png
[image07]: ./images/session04-image07.png
[image08]: ./images/session04-image08.png
[image09]: ./images/session04-image09.png
[image10]: ./images/session04-image10.png
[image11]: ./images/session04-image11.png
[image12]: ./images/session04-image12.png
[image13]: ./images/session04-image13.png
[image14]: ./images/session04-image14.png
[image15]: ./images/session04-image15.png
[image16]: ./images/session04-image16.png
[image17]: ./images/session04-image17.png
[image18]: ./images/session04-image18.png
[image19]: ./images/session04-image19.png
[image20]: ./images/session04-image20.png
[image21]: ./images/session04-image21.png
[image22]: ./images/session04-image22.png
[image23]: ./images/session04-image23.png
[image24]: ./images/session04-image24.png
[image25]: ./images/session04-image25.png
[image26]: ./images/session04-image26.png
[image27]: ./images/session04-image27.png
[image28]: ./images/session04-image28.png
[image29]: ./images/session04-image29.png
[image30]: ./images/session04-image30.png
[image31]: ./images/session04-image31.png
[image32]: ./images/session04-image32.png
[image33]: ./images/session04-image33.png
[image34]: ./images/session04-image34.png
[image35]: ./images/session04-image35.png
[image36]: ./images/session04-image36.png
[image37]: ./images/session04-image37.png
[image38]: ./images/session04-image38.png
[image39]: ./images/session04-image39.png
[image40]: ./images/session04-image40.png
[image41]: ./images/session04-image41.png
[image42]: ./images/session04-image42.png
[image43]: ./images/session04-image43.png
[image44]: ./images/session04-image44.png
[image45]: ./images/session04-image45.png
[image46]: ./images/session04-image46.png
[image47]: ./images/session04-image47.png
[image48]: ./images/session04-image48.png
[image49]: ./images/session04-image49.png
[image50]: ./images/session04-image50.png
[image51]: ./images/session04-image51.png
[image52]: ./images/session04-image52.png
[image53]: ./images/session04-image53.png
[image54]: ./images/session04-image54.png


[az ad]: https://learn.microsoft.com/ko-kr/azure/active-directory/fundamentals/active-directory-whatis?WT.mc_id=dotnet-87051-juyoo
[az ad oauth2]: https://learn.microsoft.com/ko-kr/azure/active-directory/fundamentals/auth-oauth2?WT.mc_id=dotnet-87051-juyoo
[az ad oauth2 flow authcode]: https://learn.microsoft.com/ko-kr/azure/active-directory/develop/v2-oauth2-auth-code-flow?WT.mc_id=dotnet-87051-juyoo

[az fncapp]: https://learn.microsoft.com/ko-kr/azure/azure-functions/functions-overview?WT.mc_id=dotnet-87051-juyoo

[az apim]: https://learn.microsoft.com/ko-kr/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-87051-juyoo

[pp apps]: https://learn.microsoft.com/ko-kr/power-apps/powerapps-overview?WT.mc_id=dotnet-87051-juyoo
[pp auto]: https://learn.microsoft.com/ko-kr/power-automate/getting-started?WT.mc_id=dotnet-87051-juyoo
[pp cuscon]: https://learn.microsoft.com/ko-kr/connectors/custom-connectors/?WT.mc_id=dotnet-87051-juyoo
