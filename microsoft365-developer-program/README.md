# Microsoft 365 개발자 프로그램 가입 #

## Microsoft Account 생성하기 ##

Microsoft 365(이하 M365) 개발자 프로그램에 가입하기 위해서는 Microsoft Account(이하 MSA)가 있어야 합니다. 아래 링크로 접속해서 MSA를 만드세요.

```text
https://account.microsoft.com
```

> **NOTE**: 이미 Microsoft Account가 있다면 건너뛰어도 좋습니다.


## Microsoft 365 개발자 프로그램 ##

회사에서 제공하는 M365 테넌트를 사용해도 되지만, 이럴 경우 회사 보안정책에 따라 파워 플랫폼의 모든 기능을 다 사용할 수 없을 수도 있습니다. 따라서, 파워 플랫폼 부트캠프에서 아무 제약사항 없이 파워 플랫폼을 사용하기 위해서는 먼저 [M365 개발자 프로그램][m365 dev program]에 가입하는 것이 좋습니다. 가입 과정이 조금 복잡하니 아래 순서대로 꼭 따라해 주세요.

1. 아래 링크로 접속합니다.

    ```text
    https://aka.ms/gppbkr/m365dev
    ```

1. MSA를 이용해 로그인합니다.
1. **Join now >** 버튼을 클릭합니다.
1. 아래 사항을 입력한 후 **Next** 버튼을 클릭합니다.

    - **Country/Region**: `Korea, South`
    - **Company**: 아무 이름
    - **Language Preference**: `English`
    - **"I accept the terms and conditions..."** 항목에 체크

1. **"What is your primary focus as a developer"** 질문에 아무거나 선택합니다.
1. **"What areas of Microsoft 365 development are you interested in? ​We will show you resources, tools, and training to help you get started."** 질문에 모두 선택합니다.
1. **Set up E5 Subscription** 링크를 클릭합니다.
1. 아래 그림과 같이 **Configurable sandbox**를 선택한 후 **Next** 버튼을 클릭합니다.

    ![Configurable Sandbox 선택][image01]

1. 아래 항목을 입력한 이후 **Continue** 버튼을 클릭합니다.

   - **Country/Region**: `Korea, South`
   - **Create username**: 관리자 username
   - **Create domain**: 테넌트 이름
   - **Password**: 패스워드
   - **Confirm Password**: 패스워드 확인

    ![테넌트 정보 입력][image02]

1. 본인 인증을 위해 아래와 같이 핸드폰 번호를 입력합니다.

   - **Country code**: `South Korea (82)`
   - **Phone number**: 본인 핸드폰 번호

    ![본인 확인용 핸드폰 번호 입력][image03]

1. 인증 번호가 오면 인증 번호를 입력합니다.

    ![본인 확인용 인증번호 입력][image04]

1. 개발자용 테넌트가 만들어집니다.

    ![개발자용 테넌트 생성중][image05]

1. 테넌트가 만들어지면 아래와 같은 화면이 보입니다. **Go to subscription** 링크를 클릭합니다.

    ![개발자용 테넌트 생성 완료][image06]

1. 앞서 관리자 계정으로 만들었던 계정의 패스워드를 입력합니다.

    ![관리자 계정 패스워드 입력][image07]

1. 아래와 같은 화면이 보이면 성공적으로 로그인 한 것입니다.

    ![M365 시작 화면][image08]

이렇게 M365 개발자 프로그램에 가입했습니다.


## 파워 앱 개발자 프로그램 가입 ##

M365 개발자 프로그램에 가입한 후 파워 플랫폼도 개발자 프로그램에 가입해서 프리미엄 기능을 무료로 사용할 수 있습니다. 파워 플랫폼 부트캠프에서 아무 제약사항 없이 파워 플랫폼을 사용하기 위해서는 먼저 [파워 플랫폼 개발자 프로그램][pp dev program]에 가입하는 것이 좋습니다.

1. 아래 링크로 접속합니다.

    ```text
    https://aka.ms/gppbkr/ppdev
    ```

1. **무료로 시작하기 >** 버튼을 클릭합니다.

    ![파워 플랫폼 개발자 프로그램 무료로 시작하기 버튼][image09]

1. 앞서 생성한 M365 개발자 프로그램의 계정 이메일 주소를 입력합니다. 이 이메일 주소는 반드시 `{{USERNAME}}@{{TENANT_NAME}}.onmicrosoft.com`과 같은 형식이어야 합니다.

    ![파워 플랫폼 개발자 프로그램 가입용 이메일 계정 확인][image10]

1. 이메일 계정을 확인했으면 **로그인** 버튼을 클릭해서 로그인합니다.

    ![파워 플랫폼 개발자 프로그램 가입용 이메일 계정으로 로그인][image11]

1. 아래와 같이 추가 정보를 입력한 후 **시작** 버튼을 클릭합니다.

   - **국가 또는 지역**: `대한민국`
   - **회사 전화 번호**: 아무 전화 번호

    ![파워 플랫폼 개발자 프로그램 가입을 위한 계정 상세 정보 입력][image12]

1. 개발자 프로그램에 가입한 후 **시작** 버튼을 클릭합니다.

    ![파워 플랫폼 개발자 프로그램 가입 완료][image13]

1. 개발자 환경이 추가된 것을 확인합니다.

    ![파워 플랫폼 개발자 환경 추가 확인][image14]

이렇게 파워 플랫폼 개발자 프로그램에 가입했습니다.

---

이제 실제로 앱을 만들어 보기로 합시다!

- 파워 앱과 파워 오토메이트를 이용한 인사관리 시스템 고도화
- [파워 플랫폼 커스텀 커넥터와 애저 백엔드 통합하기](../custom-connectors-in-a-day)


[image01]: ./images/image01.png
[image02]: ./images/image02.png
[image03]: ./images/image03.png
[image04]: ./images/image04.png
[image05]: ./images/image05.png
[image06]: ./images/image06.png
[image07]: ./images/image07.png
[image08]: ./images/image08.png
[image09]: ./images/image09.png
[image10]: ./images/image10.png
[image11]: ./images/image11.png
[image12]: ./images/image12.png
[image13]: ./images/image13.png
[image14]: ./images/image14.png


[m365 dev program]: https://aka.ms/gppbkr/m365dev
[pp dev program]: https://aka.ms/gppbkr/ppdev
