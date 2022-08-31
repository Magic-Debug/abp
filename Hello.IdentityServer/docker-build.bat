@echo off
echo >>Build Image<<
docker build -t  wjkhappy14/hello.ids4 .

echo >>Push Image<<
docker push wjkhappy14/hello.ids4:latest

echo. & pause