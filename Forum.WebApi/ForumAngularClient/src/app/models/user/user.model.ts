import {TopicModel} from "../topic/topic.model";
import {CommentModel} from "../comment/comment.model";

export class UserModel{
  constructor(
    public id: number = 0,
    public userName?: string,
    public email?:string,
    public birthDate?: string,
    public karma?: number,
    public moderatedCommunityId?: number,
    public createdCommunityId?: number,
    public communitiesIds?: number[],
    public postModels?: TopicModel[],
    public commentModels?: CommentModel[],
    public token?: string
  ) {
  }
}
