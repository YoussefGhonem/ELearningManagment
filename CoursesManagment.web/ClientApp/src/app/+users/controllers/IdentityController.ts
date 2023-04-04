export const IdentityController = {
  Login: `account/login`,
  ForgetPassword: (email: string) => `identity/${email}/forget-password`,
  ResetPassword: (email: string) => `identity/${email}/reset-password`,
  ChangePassword: 'identity/change-password',
}
