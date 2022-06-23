import {TopicModel} from "../topic/topic.model";
import {RuleModel} from "../rule/rule.model";
import {UserModel} from "../user/user.model";

export class CommunityModel {
  constructor(
    public id?: number,
    public title?: string,
    public about?: string,
    public creatorId?: number,
    public creationDate?: string,
    public membersCount?: number,
    public postModels?: TopicModel[],
    public ruleModels?: RuleModel[],
    public moderatorModels?: UserModel[]
  ) {
  }
}
