setlocal

set PROTOC=..\packages\Grpc.Tools.1.8.0\tools\windows_x64\protoc.exe
set PLUGIN=..\packages\Grpc.Tools.1.8.0\tools\windows_x64\grpc_csharp_plugin.exe

%PROTOC% -I..\Protobuf --csharp_out ..\Smith.Proto ProtoSmith.proto --grpc_out ..\Smith.Proto --plugin=protoc-gen-grpc=%PLUGIN%

pause