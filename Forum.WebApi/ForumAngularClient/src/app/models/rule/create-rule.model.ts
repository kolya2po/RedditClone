export class CreateRuleModel {
  constructor(
    public title?: string,
    public ruleText?: string,
    public communityId?: number
  ) {
  }
}
