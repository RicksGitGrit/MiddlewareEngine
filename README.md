# CyclicalMiddlewareEngine
Skeleton project to as a prototype for handling robot-specific logic, with flexibilty through scoped, disposable services and concurrent message handling.
# OIL
  Setup
- make functional in dummy form
- Design software architecture from notification service to MiddleWareEngine
- Add local eventbus between processing cycles (so they can interact)
- Add RabbitMq eventbus
- Add test project
  
  Flow
- Make a default flow (just push the message through on rabbitMq to another service, which we are going to validate)
- Make a prompt flow to wrap a python client that has a VLM
- Make a computer vision (CV) flow with a hold on the VLM results and process it into 3d

  Flow connection
- Make a hold on default flow for the CV results as validation input
- Connect processing cycles by pushing events on prompt return from prompt flow to CV flow
