# Session 01: 백엔드 API 개발하기

이번 워크샵에서 사용할 백엔드 API를 만들어 보기로 합니다. 여기서는 [ASP.NET Core Minimal API][aspnet minimal api]를 사용합니다.

## 백엔드 API 프로젝트 설정

1. 아래 명령어를 차례로 실행시켜 백엔드 API 프로젝트를 생성합니다.

    ```bash
    dotnet new sln -n ApiApp
    dotnet new webapi -n ApiApp -o src/ApiApp --use-minimal-apis
    dotnet sln add ./src/ApiApp -s src
    dotnet restore && dotnet build
    ```

1. 아래 명령어를 실행시켜 백엔드 API 프로젝트를 실행합니다.

    ```bash
    dotnet watch run --project ./src/ApiApp
    ```

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

---

축하합니다! 백엔드 API 개발이 끝났습니다. 이제 [Session 02: 애저 Dev CLI 이용해서 애저 인스턴스 만들기](./02-azd.md)로 넘어가세요.

> **NOTE**: 만약 백엔드 API 개발을 마치지 못했다면 [여기](./completed)에서 코드를 확인할 수 있습니다.

[aspnet minimal api]: https://learn.microsoft.com/ko-kr/aspnet/core/fundamentals/minimal-apis/overview?WT.mc_id=dotnet-87051-juyoo
