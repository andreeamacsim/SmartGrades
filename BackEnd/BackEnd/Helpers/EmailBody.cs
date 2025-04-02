namespace BackEnd.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"
        <html>
            <head> </head>
            <body style=""margin:0;padding:0;font-family: Arial, Helvetica, sans-serif;"">
                <div style height: auto; background: linear gradient(to top, #c9c9ff 50%, #6eef6 90%) no-repeat width:400px; padding: 30px;>
                    <div>
                        <div>
                            <h1> Reset your Password </h1>
                            <hr>
                            <p>You're receiving this email because you requested a password reset for your Smart Grades account.</p>
                            <p>Click the button below to reset your password. If you didn't request a password reset, you can ignore this email.</p>
                            <a href=""http://localhost:5070/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background-color: #0d6efd;padding:10px;border:none 
                            color: white; border-radius:4px;display:block;margin:0 auto;width:50%;text-align:center;padding:10px;text-decoration:none;"">Reset Password</a>

                            <p>Kindest regards,<br><br>Smart Grades Team</p>
                        </div>
                    </div>
                </div>
            </body>
        </html>";
        }
    }
}
