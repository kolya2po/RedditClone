export class UpdateCommentModel {
  constructor(
    public id?: number,
    public authorId?: number,
    public topicId?: number,
    public text?: string,
    public postingDate?: string
  ) {
  }
}
