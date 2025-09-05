## Usage

    private static void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOidc(
            configuration["AuthServer:Authority"]!,
            ["{--PROJECT--}"],
            [AbpSwaggerOidcFlows.AuthorizationCode],
            null,
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "{--PROJECT--API--}", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                //options.CustomSchemaIds(type => type.FullName); // Used for angular abp io packages
                options.CustomSchemaIds(type => type.FriendlyId(false)); // this is for Flutter
            });
    }