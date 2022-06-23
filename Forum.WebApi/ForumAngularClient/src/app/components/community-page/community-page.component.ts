import { Component, OnInit } from '@angular/core';
import {CommunityModel} from "../../models/community/community.model";
import {CommunityService} from "../../services/community.service";
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "../../services/user.service";
import {TopicService} from "../../services/topic.service";
import {TopicModel} from "../../models/topic/topic.model";
import {TopicHelperService} from "../../services/topic-helper.service";

@Component({
  selector: 'app-community-page',
  templateUrl: './community-page.component.html',
  styleUrls: ['./community-page.component.css']
})
export class CommunityPageComponent implements OnInit {
  community = new CommunityModel();
  topicsCopy: TopicModel[] = [];
  id = 0;
  private sub: any
  private isSorted = false;
  constructor(public us: UserService,
              private cs: CommunityService,
              private route: ActivatedRoute,
              public router: Router,
              private ts: TopicService,
              private topicHelper: TopicHelperService) { }

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
          // @ts-ignore
          this.topicsCopy = [...this.community.postModels];
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

  increase(topic: any) {
    if (this.us.user.id === 0) {
      this.router.navigate(['/login']);
      return;
    }
    this.topicHelper.increaseRating(topic, this.community.postModels);
  }

  decrease(topic: any) {
    if (this.us.user.id === 0) {
      this.router.navigate(['/login']);
      return;
    }
    this.topicHelper.decreaseRating(topic, this.community.postModels);
  }

  sortTop() {
    let obj = this.topicHelper.sortTop(this.isSorted, this.community.postModels, this.topicsCopy);
    this.community.postModels = obj.posts;
    this.isSorted = obj.isSorted;
    this.topicsCopy = obj.postsCopy;
  }

  sortNew() {
    let obj = this.topicHelper.sortNew(this.isSorted, this.community.postModels, this.topicsCopy);
    this.community.postModels = obj.posts;
    this.isSorted = obj.isSorted;
    this.topicsCopy = obj.postsCopy;
  }

  deleteTopic(id: any) {
    this.ts.delete(id)
      .subscribe(() => {
        this.community.postModels = this.community.postModels
          ?.filter(p => p.id !== id);

      });
  }

  pin(topic: any) {
    this.ts.pin(topic.id)
      .subscribe(() => {
        topic.isPinned = true;
        this.community.postModels = this.community.postModels
          ?.filter(t => t.id !== topic.id);
        this.community.postModels?.unshift(topic);
        if (this.isSorted) {
          let pinned = this.community.postModels!.filter(t => t.isPinned);
          let notPinned = this.topicsCopy.filter(t => !t.isPinned && !pinned.includes(t));
          this.topicsCopy = pinned.concat(notPinned);
        }
      })
  }

  unpin(topic: any) {
    this.ts.unpin(topic.id)
      .subscribe(() => {
        topic.isPinned = false;
        this.community.postModels = this.community.postModels
          ?.filter(t => t.id !== topic.id);
        this.community.postModels?.push(topic);

        if (this.isSorted) {
          let pinned = this.community.postModels!.filter(t => t.isPinned);
          let notPinned = this.topicsCopy.filter(t => !t.isPinned && !pinned.includes(t));
          this.topicsCopy = pinned.concat(notPinned);
        }
      })
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
