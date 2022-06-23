export class UpdateRuleModel {
  constructor(
    public id?: number,
    public title?: string,
    public ruleText?: string,
    public communityId?: number
  ) {
  }
}
