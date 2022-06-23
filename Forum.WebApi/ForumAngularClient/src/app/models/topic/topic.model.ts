import {CommentModel} from "../comment/comment.model";

export class TopicModel {
  constructor(
    public id?: number,
    public title?: string,
    public text?: string,
    public rating?: number,
    public isPinned?: boolean,
    public commentsAreBlocked?: boolean,
    public postingDate?: string,
    public commentsCount?: number,
    public commentModels?: CommentModel[],
    public authorId?: number,
    public authorName?: string,
    public communityId?: number,
    public communityName?: string,
    public isRatedUp: boolean = false,
    public isRatedDown: boolean = false
  ) {
  }
}
