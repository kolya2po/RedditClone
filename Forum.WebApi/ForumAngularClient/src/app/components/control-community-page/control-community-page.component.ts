import { Component, OnInit } from '@angular/core';
import {CommunityService} from "../../services/community.service";
import {RouterExtService} from "../../routing/routerExt.service";
import {CommunityModel} from "../../models/community/community.model";
import {UpdateCommunityModel} from "../../models/community/update-community.model";
import {CreateRuleModel} from "../../models/rule/create-rule.model";
import {UpdateRuleModel} from "../../models/rule/update-rule.model";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-control-community-page',
  templateUrl: './control-community-page.component.html',
  styleUrls: ['./control-community-page.component.css']
})
export class ControlCommunityPageComponent implements OnInit {
  sub: any;
  id: any;
  community = new CommunityModel();
  updateCommunityModel = new UpdateCommunityModel();

  createRuleModel = new CreateRuleModel();
  updateRuleModel = new UpdateRuleModel();
  isRuleCreating = false;
  isRuleUpdating = false;

  constructor(private cs: CommunityService,
              private routeExt: RouterExtService,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(p => {
      this.id = +p['id'];

      this.cs.getById(this.id)
        .subscribe(c => {
          this.community = c;
          this.updateCommunityModel.id = c.id;
          this.updateCommunityModel.creationDate = c.creationDate;
          this.updateCommunityModel.title = c.title;
          this.updateCommunityModel.about = c.about;
        });
    })
  }

  updateCommunity() {
    this.cs.update(this.updateCommunityModel)
      .subscribe(() => {
        this.community.title = this.updateCommunityModel.title;
      });
  }


  createRule() {
    this.createRuleModel.communityId = this.id;
    this.cs.addRule(this.createRuleModel)
      .subscribe(r => {
        this.community.ruleModels?.push(r);
        this.isRuleCreating = false;
        this.createRuleModel = new CreateRuleModel();
      })
  }

  startEditing(r: any) {
    this.isRuleUpdating = true;
    this.updateRuleModel.id = r.id;
    this.updateRuleModel.title = r.title;
    this.updateRuleModel.ruleText = r.ruleText;
    this.updateRuleModel.communityId = this.id;
  }

  updateRule() {
    this.cs.updateRule(this.updateRuleModel)
      .subscribe(() => {
        let rule = this.community.ruleModels?.find(r => r.id === this.updateRuleModel.id);
        rule!.title = this.updateRuleModel.title;
        rule!.ruleText = this.updateRuleModel.ruleText;
        this.isRuleUpdating = false;
        this.updateRuleModel = new CreateRuleModel();
      })
  }

  deleteRule(id: any) {
    this.cs.deleteRule(id)
      .subscribe(() => {
        this.community.ruleModels = this.community.ruleModels
          ?.filter(c => c.id !== id);
      });
  }

  goToPreviousPage() {
    let prev = this.routeExt.getPreviousUrl();
    return this.routeExt.router.navigate([prev]);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
    this.updateCommunity();
  }
}
