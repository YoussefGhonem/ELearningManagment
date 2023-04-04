export class UserModel {
  id?: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  name?: string;
  imageUrl?: string;
  isActive?: string;
  emailConfirmed?: boolean;
  roles: string[];
}
