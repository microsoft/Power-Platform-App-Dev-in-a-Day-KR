# Session 01: 백엔드 API 개발하기

이번 워크샵에서 사용할 백엔드 API를 만들어 보기로 합니다. 여기서는 [ASP.NET Core Minimal API][aspnet minimal api]를 사용합니다.

## 백엔드 API 프로젝트 설정

1. 아래 명령어를 통해 작업 디렉토리로 이동합니다. 이곳이 이번 세션에서 사용할 디렉토리입니다.

   ```bash
   cd $CODESPACE_VSCODE_FOLDER/devops-in-a-day
   ```

1. 아래 명령어를 차례로 실행시켜 백엔드 API 프로젝트를 생성합니다.

    ```bash
    # ASP.NET 솔루션 파일 생성
    dotnet new sln -n ApiApp

    # ASP.NET 백엔드 API 프로젝트 생성
    dotnet new webapi -n ApiApp -o src/ApiApp --use-minimal-apis

    # 솔루션 파일에 프로젝트 추가
    dotnet sln add ./src/ApiApp -s src

    # 프로젝트 빌드
    dotnet restore && dotnet build
    ```

1. 아래 명령어를 실행시켜 백엔드 API 프로젝트를 실행합니다.

    ```bash
    dotnet watch run --project ./src/ApiApp
    ```

   > **NOTE**: 만약 브라우저의 새 탭이 열려서 Swagger UI가 열리지 않았다면 주소창의 맨 마지막에 `/swagger`를 추가한 후 다시 실행시킵니다.

1. 터미널에서 Ctrl + C 또는 Cmd + C 키를 눌러 백엔드 API 프로젝트를 종료합니다.

1. `src/ApiApp/Program.cs` 파일을 열고 아래와 같이 수정합니다.

    ```csharp
    // 수정 전
    builder.Services.AddSwaggerGen();
    
    // 수정 후
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "GPPB Backend API", Version = "v1" });
    });
    ```

1. `src/ApiApp/Program.cs` 파일을 열고 아래와 같이 수정합니다.

    ```csharp
    // 수정 전
    app.MapGet("/weatherforecast", () =>
    {
        ...
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();
    
    // 수정 후
    app.MapGet("/weatherforecast", () =>
    {
        ...
    })
    .WithTags("weather") // 👈 추가
    .WithName("GetWeatherForecast")
    .WithOpenApi();
    ```

1. 아래 명령어를 실행시켜 백엔드 API 프로젝트를 실행하여 변경 사항을 확인합니다.

    ```bash
    dotnet watch run --project ./src/ApiApp
    ```

   > **NOTE**: 만약 브라우저의 새 탭이 열려서 Swagger UI가 열리지 않았다면 주소창의 맨 마지막에 `/swagger`를 추가한 후 다시 실행시킵니다.

1. `GetWeatherForecast` API를 클릭하고 `Try it out` 버튼을 클릭한 후 `Execute` 버튼을 클릭합니다. 데이터가 잘 나오는지 확인합니다.

1. 터미널에서 `Ctrl + C` 또는 `Cmd + C` 키를 눌러 백엔드 API 프로젝트를 종료합니다.

---

축하합니다! 백엔드 API 개발이 끝났습니다. 이제 [Session 02: 애저 Dev CLI 이용해서 애저 인스턴스 만들기](./02-azd.md)로 넘어가세요.

> **NOTE**: 만약 백엔드 API 개발을 마치지 못했다면 [여기](./completed)에서 코드를 확인할 수 있습니다.

[aspnet minimal api]: https://learn.microsoft.com/ko-kr/aspnet/core/fundamentals/minimal-apis/overview?WT.mc_id=dotnet-87051-juyoo
