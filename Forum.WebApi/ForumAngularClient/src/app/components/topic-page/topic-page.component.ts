import { Component, OnInit } from '@angular/core';
import {TopicModel} from "../../models/topic/topic.model";
import {UserService} from "../../services/user.service";
import {TopicService} from "../../services/topic.service";
import {ActivatedRoute} from "@angular/router";
import {RouterExtService} from "../../routing/routerExt.service";
import {CommentService} from "../../services/comment.service";
import {CreateCommentModel} from "../../models/comment/create-comment.model";
import {MatSnackBar} from "@angular/material/snack-bar";
import {UpdateCommentModel} from "../../models/comment/update-comment.model";
import {CommentHelperService} from "../../services/comment-helper.service";
import {TopicHelperService} from "../../services/topic-helper.service";

@Component({
  selector: 'app-topic-page',
  templateUrl: './topic-page.component.html',
  styleUrls: ['./topic-page.component.css']
})
export class TopicPageComponent implements OnInit {
  sub: any;
  id = 0;
  topic = new TopicModel();
  isReply = false;
  idToReply = 0;
  isSpaceOnly = false;
  createCommentPlaceholder = 'Comment here';
  createComment = new CreateCommentModel('');
  updateCommentModel = new UpdateCommentModel();

  constructor(public us: UserService, private ts: TopicService,
              private route: ActivatedRoute,
              private routeExt: RouterExtService,
              private cs: CommentService,
              private _snackBar: MatSnackBar,
              private commentHelper: CommentHelperService,
              private topicHelper: TopicHelperService) { }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(p => {
      this.id = +p['id'];

      this.ts.getById(this.id)
        .subscribe(t => this.topic = t);
    });
  }

  increaseTopicRating() {
    if (this.us.user.id === 0) {
      this.openSnackBar();
      return;
    }
    this.topicHelper.increaseRating(this.topic, undefined);
  }

  decreaseTopicRating() {
    if (this.us.user.id === 0) {
      this.openSnackBar();
      return;
    }
   this.topicHelper.decreaseRating(this.topic, undefined);
  }

  increaseCommentRating(comment: any) {
    if (this.us.user.id === 0) {
      this.openSnackBar();
      return;
    }
    this.commentHelper.increaseRating(comment, this.topic.commentModels);
  }

  decreaseCommentRating(comment: any) {
    if (this.us.user.id === 0) {
      this.openSnackBar();
      return;
    }
    this.commentHelper.decreaseRating(comment, this.topic.commentModels);
  }

  goToPreviousPage() {
    let prev = this.routeExt.getPreviousUrl();
    return this.routeExt.router.navigate([prev]);
  }

  leaveComment() {
    this.verifyComment();
    this.createComment.topicId = this.topic.id;
    this.createComment.authorId = this.us.user.id;

    if (!this.isReply) {
      this.cs.add(this.createComment)
        .subscribe(() => {
          this.ts.getById(this.id)
            .subscribe(t => this.topic = t);
        });

    } else {
      this.cs.reply(this.idToReply, this.createComment)
        .subscribe(() => {
          this.ts.getById(this.id)
            .subscribe(t => this.topic = t);
      });
      window.scrollTo({
        top: 1700,
        left: 0,
        behavior: 'smooth'
      });
    }

    // @ts-ignore
    this.topic.commentsCount++;
    this.createComment = new CreateCommentModel();
    this.isSpaceOnly = false;
    this.isReply = false;
  }

  blockComments() {
    this.ts.blockComments(this.topic.id)
      .subscribe(() => this.topic.commentsAreBlocked = true);
  }

  replyToComment(id: any, userName: any) {
    window.scroll(0, 0);
    this.createCommentPlaceholder = `Write reply to user ${userName}`;
    this.idToReply = id;
    this.isReply = true;
  }

  startEditingComment(comment: any) {
    comment.isEditing = true;
    this.updateCommentModel.id = comment.id;
    this.updateCommentModel.text = comment.text;
    this.updateCommentModel.authorId = comment.authorId;
    this.updateCommentModel.topicId = comment.topicId;
    this.updateCommentModel.postingDate = comment.postingDate;
  }

  updateComment(comment: any) {
    this.cs.update(this.updateCommentModel)
      .subscribe(() => {
        comment.text = this.updateCommentModel.text;
        comment.isEditing = false;
      })
  }

  deleteComment(id: any) {
    this.cs.delete(id)
      .subscribe(() => {
        this.topic.commentModels = this.topic.commentModels?.filter(c => c.id !== id);
        // @ts-ignore
        this.topic.commentsCount--;
      });
  }

  showDelete(authorId: any) {
    return this.us.user.id === authorId ||
           this.us.user.moderatedCommunityId === this.topic.communityId
  }

  verifyComment() {
    if (this.createComment.text?.trim().length === 0) {
      this.isSpaceOnly = true;
      return;
    }
  }

  openSnackBar() {
    this._snackBar.open('Please, login.', '', {
      duration: 2000
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
