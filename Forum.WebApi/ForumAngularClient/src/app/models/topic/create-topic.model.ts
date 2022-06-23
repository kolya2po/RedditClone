export class CreateTopicModel {
  constructor(
    public title?: string,
    public text?: string,
    public authorId?: number,
    public communityId?: number,
  ) {
  }
}
