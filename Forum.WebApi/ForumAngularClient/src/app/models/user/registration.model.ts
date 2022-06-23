export class RegistrationModel {
  constructor(
    public email?: string,
    public userName?: string,
    public password?: string,
    public birthDate?: Date,
  ) {
  }
}
