import {Component, Input} from '@angular/core';
import {UserService} from "../../services/user.service";
import {Router} from "@angular/router";
import {TopicHelperService} from "../../services/topic-helper.service";
import {TopicModel} from "../../models/topic/topic.model";
import {MatSnackBar} from "@angular/material/snack-bar";
import {CommunityService} from "../../services/community.service";
import {TopicService} from "../../services/topic.service";
import {CommunityModel} from "../../models/community/community.model";
import {UserModel} from "../../models/user/user.model";
import {SearchService} from "../../services/search.service";

@Component({
  selector: 'app-topic-list',
  templateUrl: './topic-list.component.html',
  styleUrls: ['./topic-list.component.css']
})
export class TopicListComponent {
  @Input() topics: TopicModel[] = [];
  @Input() isMainPage = false;
  @Input() isUserPage = false;
  @Input() community: CommunityModel = new CommunityModel();
  @Input() user: UserModel = new UserModel();
  topicsCopy: TopicModel[] = [];
  isSorted = false;
  search = '';

  constructor(public us: UserService, public router: Router,
              private topicHelper: TopicHelperService,
              private _snackBar: MatSnackBar,
              private cs: CommunityService,
              private ts: TopicService,
              private ss: SearchService) { }

  ngOnChanges(): void {
    this.topicsCopy = [...this.topics];
    this.ss.currentSearch.subscribe(s => this.search = s);
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
    if (!this.isMainPage) {
      return false;
    }
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
    let obj = this.topicHelper.sortTop(this.isSorted, this.topics, this.topicsCopy, this.isMainPage, this.isUserPage);
    // @ts-ignore
    this.topics = obj.posts;
    this.topicsCopy = obj.postsCopy;
    this.isSorted = obj.isSorted;
  }

  sortNew() {
    let obj = this.topicHelper.sortNew(this.isSorted, this.topics, this.topicsCopy, this.isMainPage, this.isUserPage);
    // @ts-ignore
    this.topics = obj.posts;
    this.topicsCopy = obj.postsCopy;
    this.isSorted = obj.isSorted;
  }

  deleteTopic(id: any) {
    this.ts.delete(id)
      .subscribe(() => {
        this.topics = this.topics
          ?.filter(p => p.id !== id);
      });
  }

  pin(topic: any) {
    this.ts.pin(topic.id)
      .subscribe(() => {
        topic.isPinned = true;
        this.topics = this.topics.filter(t => t.id !== topic.id);
        this.topics.unshift(topic);
        if (this.isSorted) {
          let pinned = this.topics.filter(t => t.isPinned);
          let notPinned = this.topicsCopy.filter(t => !t.isPinned && !pinned.includes(t));
          this.topicsCopy = pinned.concat(notPinned);
        }
      })
  }

  unpin(topic: any) {
    this.ts.unpin(topic.id)
      .subscribe(() => {
        topic.isPinned = false;
        this.topics = this.topics.filter(t => t.id !== topic.id);
        this.topics.push(topic);

        if (this.isSorted) {
          let pinned = this.topics.filter(t => t.isPinned);
          let notPinned = this.topicsCopy.filter(t => !t.isPinned && !pinned.includes(t));
          this.topicsCopy = pinned.concat(notPinned);
        }
      })
  }

  openSnackBar() {
    this._snackBar.open('Please, login.', '', {
      duration: 2000
    });
  }
}
