export class CommentModel {
  constructor(
    public id?: number,
    public text?: string,
    public rating?: number,
    public authorId?: number,
    public postingDate?: string,
    public authorName?: string,
    public topicId?: number,
    public topicName?: string,
    public communityName?: string,
    public isEditing: boolean = false,
    public isRatedUp: boolean = false,
    public isRatedDown: boolean = false
  ) {
  }
}
