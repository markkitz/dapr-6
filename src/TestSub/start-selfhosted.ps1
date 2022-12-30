dapr run `
    --app-id testsub `
    --app-port 5228 `
    --dapr-http-port 50028 `
    --dapr-grpc-port 60028 `
    --config ../dapr/components/config.yaml `
    --components-path ../dapr/components `
    dotnet run