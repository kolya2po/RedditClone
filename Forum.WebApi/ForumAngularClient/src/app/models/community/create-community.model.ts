export class CreateCommunityModel {
  constructor(
    public title?: string,
    public about?: string,
    public creatorId?: number,
  ) {
  }
}
