export class UpdateTopicModel {
  constructor(
    public id?: number,
    public title?: string,
    public text?: string,
    public authorId?: number,
    public communityId?: number,
    public postingDate?: string,
    public rating?: number,
  ) {
  }
}
