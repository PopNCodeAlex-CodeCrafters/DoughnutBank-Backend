## Implementation

### Api Authorization
I created a static API key which should be passed in the header of every request. Saved in `appsettings.Development.json`
No functionality of this API can be used without the `API key`. Created and registered `ApiAccessMiddleware` to extract the api key from the
http request and compare it to the api key set in appsettings.

Added to swagger the interface needed to be able to test with authorization.



# Technical Challenge

## The problem

Develop a system that can generate a one-time password for a banking application. The OTP system must be secure, efficient, and user-friendly to enhance the user experience and protect customers' data.
 
## Business Requirements

1. The OTP system must be secure to protect the confidential data of the customers. It must ensure that OTPs are generated randomly and are not predictable. Encryption during transmission of the OTPs should also be ensured.
2. The OTPs should be time-bound. Once generated, an OTP should not be valid indefinitely. The system should automatically invalidate the OTP after a certain period of time that can be easily customized.
3. The OTP input interface should be user-friendly. It should allow users to input the OTP easily without any confusion.
4. The system should have good error handling. It should inform the user about any issues in a clear and understandable way.
5. For the purpose of this exercise, the user should receive the OTP in a toast message that will be visible as long as the OTP is valid.

## Techincal requirements

1. The solution must be a web application developed on the latest .NET framework.
2. For the frontend part you can use any javascript framework you want.
3. Unit tests must be performed with a test coverage of at least 70%.

## Evaluation
Your solution will be evaluated based on coding and testability standards, naming conventions, project structure and the meeting of requirements. It must be uploaded in a public github repository that can be accessed by us for validation.


 
