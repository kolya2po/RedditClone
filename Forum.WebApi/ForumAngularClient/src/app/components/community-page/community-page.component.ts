import { Component, OnInit } from '@angular/core';
import {CommunityModel} from "../../models/community/community.model";
import {CommunityService} from "../../services/community.service";
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "../../services/user.service";
import {TopicModel} from "../../models/topic/topic.model";

@Component({
  selector: 'app-community-page',
  templateUrl: './community-page.component.html',
  styleUrls: ['./community-page.component.css']
})
export class CommunityPageComponent implements OnInit {
  community = new CommunityModel();
  topics: TopicModel[] = [];
  id = 0;
  private sub: any
  constructor(public us: UserService,
              private cs: CommunityService,
              private route: ActivatedRoute,
              public router: Router) { }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(p => {
      this.id = +p['id'];
      this.cs.getById(this.id)
        .subscribe(c => {
          this.community = c;
          let pinnedTopics = c.postModels?.filter(t => t.isPinned);
          this.community.postModels = this.community.postModels
            ?.filter(t => !t.isPinned);
          // @ts-ignore
          this.community.postModels = pinnedTopics.concat(this.community.postModels);
          this.topics = [...this.community.postModels];
        });
    });
  }

  join() {
    if (this.us.user.id === 0) {
      this.router.navigate(['login']);
      return;
    }
    this.cs.join(this.us.user.id, this.id)
      .subscribe(() => {
        this.us.user.communitiesIds?.push(this.id);
        // @ts-ignore
        this.community.membersCount++;
      });
  }

  leave() {
    this.cs.leave(this.us.user.id, this.id)
      .subscribe(() => {
        this.us.user.communitiesIds = this.us.user.communitiesIds?.filter(id => id !== this.id);
        // @ts-ignore
        this.community.membersCount--;
      });
  }

  showJoin() {
    let userInCommunity = this.us.user.communitiesIds?.includes(this.id);
    return this.us.user.id === 0 || (this.us.user.id !== 0 && !userInCommunity);
  }

  showLeave() {
    let userInCommunity = this.us.user.communitiesIds?.includes(this.id);
    return this.us.user.id !== 0 && userInCommunity && this.us.user.moderatedCommunityId !== this.id;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
