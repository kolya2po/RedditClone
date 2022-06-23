import { Component, OnInit } from '@angular/core';
import {TopicModel} from "../../models/topic/topic.model";
import {UserService} from "../../services/user.service";
import {Router} from "@angular/router";
import {CommunityService} from "../../services/community.service";
import {CommunityModel} from "../../models/community/community.model";
import {TopicService} from "../../services/topic.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {TopicHelperService} from "../../services/topic-helper.service";

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {
  topics: TopicModel[] = [];
  topicsCopy: TopicModel[] = [];
  isSorted = false;
  communities: CommunityModel[] = [];

  constructor(public us: UserService, public router: Router,
              private cs: CommunityService,
              private ts: TopicService,
              private _snackBar: MatSnackBar,
              private topicHelper: TopicHelperService) { }

  ngOnInit(): void {
    this.ts.getAll()
      .subscribe(t => {
        this.topics = t;
        this.topicsCopy = [...this.topics];
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

  increase(topic: any) {
    if (this.us.user.id === 0) {
      this.openSnackBar();
      return;
    }
    this.topicHelper.increaseRating(topic, this.topics);
  }

  decrease(topic: any) {
    if (this.us.user.id === 0) {
      this.openSnackBar();
      return;
    }
    this.topicHelper.decreaseRating(topic, this.topics);
  }

  sortTop() {
    let obj = this.topicHelper.sortTop(this.isSorted, this.topics, this.topicsCopy);
    this.topics = obj.posts;
    this.topicsCopy = obj.postsCopy;
    this.isSorted = obj.isSorted;
  }

  sortNew() {
    let obj = this.topicHelper.sortNew(this.isSorted, this.topics, this.topicsCopy);
    this.topics = obj.posts;
    this.topicsCopy = obj.postsCopy;
    this.isSorted = obj.isSorted;
  }

  openSnackBar() {
    this._snackBar.open('Please, login.', '', {
      duration: 2000
    });
  }
}
