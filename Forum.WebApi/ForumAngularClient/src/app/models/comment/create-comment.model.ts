export class CreateCommentModel {
  constructor(
    public text?: string,
    public authorId?: number,
    public topicId?: number
  ) {
  }
}
