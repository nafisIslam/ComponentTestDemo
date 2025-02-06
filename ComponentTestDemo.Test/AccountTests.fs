namespace ComponentTestDemo.Test

module AccountTests =
  open Expecto
  open Steps
  open AccountSteps

  let test = 
    testList "Account Tests"
      [
        testList "User Account Saga"
          [
            testCaseWithCleanup "User Can Register and Verify then Login and Logout" <| fun _ ->
              ``without auth`` ()
                |> ``given``
                |> ``user can register with username, email & password`` "Nafis Islam" "nafis0014@gmail.com" "12345"
                |> ``then``
                |> ``user can login`` "nafis0014@gmail.com" "12345" "Nafis Islam"
                |> ignore

            testCaseWithCleanup "When User Tries To Register With Same Email 2 Times He Gets An Error" <| fun _ ->
              ``without auth`` ()
                |> ``given``
                |> ``user can register with username, email & password`` "Nafis Islam" "nafis0014@example.com" "12345"
                |> ``then``
                |> ``user can login`` "nafis0014@example.com" "12345" "Nafis Islam"
                |> ``then``
                |> ``user tries to register with same email, he gets an error`` "Waba Laba Dub Dub" "nafis0014@example.com" "456789"
                |> ignore

            testCaseWithCleanup "User SignsIn -> tries to access authorized api -> he can" <| fun _ ->
              ``without auth`` ()
              |> ``when``
              |> ``user registers then signs in``
              |> ``then``
              |> ``he can access authorized api`` "WeatherForecast"
              |> ignore
            
            testCaseWithCleanup "User can not access authorized api with out loging in" <| fun _ ->
              ``without auth`` ()
              |> ``when``
              |> ``user is not loged in``
              |> ``then``
              |> ``he can not access authorized api`` "WeatherForecast"
              |> ignore

            testCaseWithCleanup "User SignsIn ->  SignsOut -> tries to access authorized api -> he can not" <| fun _ ->
              ``without auth`` ()
              |> ``when``
              |> ``user registers then signs in``
              |> ``then``
              |> ``user logsout``
              |> ``then``
              |> ``he can not access authorized api`` "WeatherForecast"
              |> ignore
          ]
      ]

