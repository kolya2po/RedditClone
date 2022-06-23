import { Component, OnInit } from '@angular/core';
import {UserModel} from "../../models/user/user.model";
import {UserService} from "../../services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {TopicService} from "../../services/topic.service";
import {CommunityService} from "../../services/community.service";
import {UpdateCommentModel} from "../../models/comment/update-comment.model";
import {CommentService} from "../../services/comment.service";
import {TopicModel} from "../../models/topic/topic.model";
import {CommentModel} from "../../models/comment/comment.model";
import {UpdateUserModel} from "../../models/user/update-user.model";
import {catchError, tap} from "rxjs";
import {TopicHelperService} from "../../services/topic-helper.service";

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.css']
})
export class UserPageComponent implements OnInit {
  user = new UserModel();
  updateUserModel = new UpdateUserModel();
  sub: any;
  id = 0;
  isEditing = false;
  updateCommentModel = new UpdateCommentModel();
  topicsSorted = false;
  commentsSorted = false;
  topicsCopy: TopicModel[] = [];
  commentsCopy: CommentModel[] = [];

  constructor(public us: UserService, private route: ActivatedRoute,
              private _snackBar: MatSnackBar,
              private ts: TopicService,
              private router: Router,
              private cs: CommunityService,
              private commentService: CommentService,
              private topicHelper: TopicHelperService) {
  }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(p => {
      this.id = +p['id'];

      this.us.getById(this.id)
        .subscribe(u => {
          this.user = u;

          this.updateUserModel.id = u.id;
          // @ts-ignore
          this.updateUserModel.userName = u.userName;
          // @ts-ignore
          this.updateUserModel.birthDate =  u.birthDate;

          // @ts-ignore
          this.topicsCopy = [...this.user.postModels];
          // @ts-ignore
          this.commentsCopy = [...this.user.commentModels];
        });
    });
  }

  increase(topic: any) {
    if (this.us.user.id === 0) {
      this.router.navigate(['/login']);
      return;
    }
    this.user.karma = this.topicHelper.increaseRating(topic, this.user.postModels, this.user.karma);
  }

  decrease(topic: any) {
    if (this.us.user.id === 0) {
      this.router.navigate(['/login']);
      return;
    }
    this.user.karma = this.topicHelper.decreaseRating(topic, this.user.postModels, this.user.karma);
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

  deleteTopic(id: any) {
    console.log(id);
    this.ts.delete(id)
      .subscribe(() => {
        this.user.postModels = this.user.postModels?.filter(t => t.id !== id);
      })
  }

  startEditing(comment: any) {
    comment.isEditing = true;
    this.updateCommentModel.id = comment.id;
    this.updateCommentModel.text = comment.text;
    this.updateCommentModel.authorId = comment.authorId;
    this.updateCommentModel.topicId = comment.topicId;
    this.updateCommentModel.postingDate = comment.postingDate;
  }

  updateComment(comment: any) {
    this.commentService.update(this.updateCommentModel)
      .subscribe(() => {
        comment.text = this.updateCommentModel.text;
        comment.isEditing = false;
      })
  }

  updateUser() {
    this.us.update(this.updateUserModel)
      .pipe(tap(() => {
      }), catchError((err) => {
        this.openSnackBar('You must be at least 7 years old.');
        throw err;
      }))
      .subscribe(() => {
        this.us.getById(this.user.id)
          .subscribe(u => {
            this.us.user = u;
            this.user.userName = u.userName;
            this.user.birthDate = u.birthDate;
          });

        this.isEditing = false;
      });
  }

  sortTopTopics() {
    let obj = this.topicHelper
      .sortTop(this.topicsSorted, this.user.postModels, this.topicsCopy);
    this.user.postModels = obj.posts;
    this.topicsSorted = obj.isSorted;
    this.topicsCopy = obj.postsCopy;
  }

  sortNewTopics() {
    let obj = this.topicHelper
      .sortNew(this.topicsSorted, this.user.postModels, this.topicsCopy);
    this.user.postModels = obj.posts;
    this.topicsSorted = obj.isSorted;
    this.topicsCopy = obj.postsCopy;
  }

  sortTopComments() {
    if (!this.commentsSorted) {
      // @ts-ignore
      this.user.commentModels.sort((a, b) => b.rating - a.rating);
      this.commentsSorted = true;
      return;
    }

    this.user.commentModels = [...this.commentsCopy];
    this.commentsSorted = false;
  }

  sortNewComments() {
    if (!this.commentsSorted) {
      // @ts-ignore
      this.user.commentModels.sort((a, b) => {
        // @ts-ignore
        return b.postingDate.localeCompare(a.postingDate);
      });
      this.commentsSorted = true;
      return;
    }

    this.user.commentModels = [...this.commentsCopy];
    this.commentsSorted = false;
  }

  deleteComment(id: any) {
    this.commentService.delete(id)
      .subscribe(() => {
        this.us.user.commentModels = this.us.user.commentModels
          ?.filter(c => c.id !== id);
        this.user.commentModels = this.user.commentModels
          ?.filter(c => c.id !== id);
    });
  }

  openSnackBar(message: any) {
    this._snackBar.open(message, '', {
      duration: 2000
    });
  }
}
