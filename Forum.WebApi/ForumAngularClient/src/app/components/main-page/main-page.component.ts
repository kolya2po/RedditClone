import { Component, OnInit } from '@angular/core';
import {TopicModel} from "../../models/topic/topic.model";
import {UserService} from "../../services/user.service";
import {Router} from "@angular/router";
import {CommunityService} from "../../services/community.service";
import {CommunityModel} from "../../models/community/community.model";
import {TopicService} from "../../services/topic.service";

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {
  topics: TopicModel[] = [];
  communities: CommunityModel[] = [];

  constructor(public us: UserService, public router: Router,
              private cs: CommunityService,
              private ts: TopicService) { }

  ngOnInit(): void {
    this.ts.getAll()
      .subscribe(t => {
        this.topics = t;
      });

    this.cs.getAll()
      .subscribe(c => this.communities = c.sort((a, b) =>
      {
         // @ts-ignore
        return b.membersCount - a.membersCount;
      }).slice(0, 5));
  }

  join(communityId: any) {
    if (this.us.user.id === 0) {
      this.router.navigate(['login']);
      return;
    }
    this.cs.join(this.us.user.id, communityId)
      .subscribe(() => this.us.user.communitiesIds?.push(communityId));
  }

  showJoin(id: any) {
    let userInCommunity = this.us.user.communitiesIds?.includes(id);
    return this.us.user.id === 0 || (this.us.user.id !== 0 && !userInCommunity);
  }
}
