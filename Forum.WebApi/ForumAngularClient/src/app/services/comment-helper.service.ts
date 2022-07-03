import {Injectable} from "@angular/core";
import {CommentModel} from "../models/comment/comment.model";
import {CommentService} from "./comment.service";

@Injectable({
  providedIn: 'root'
})
export class CommentHelperService {

  constructor(private cs: CommentService) {
  }

  increaseRating(comment: any, comments: CommentModel[] | undefined) {
    if (comment.isRatedDown && !comment.isRatedUp) {
      this.increase(comment, comments);
      this.increase(comment, comments);
      comment.isRatedDown = false;
      comment.isRatedUp = true;
      return;
    }

    if (!comment.isRatedUp || comment.isRatedDown) {
      this.increase(comment, comments);
      comment.isRatedUp = true;
      return;
    }

    this.decrease(comment, comments);
    comment.isRatedUp = false;
  }

  increase(comment: any, comments: CommentModel[] | undefined) {
    this.cs.increaseRating(comment.id)
      .subscribe(() => {
        // @ts-ignore
        let index = comments.findIndex(c => c.id == comment.id);
        // @ts-ignore
        comments[index].rating++;
      });
  }

  decreaseRating(comment: any, comments: CommentModel[] | undefined) {
    if (!comment.isRatedDown && comment.isRatedUp) {
      this.decrease(comment, comments);
      this.decrease(comment, comments);
      comment.isRatedDown = true;
      comment.isRatedUp = false;
      return;
    }
    if (!comment.isRatedDown) {
      this.decrease(comment, comments);
      comment.isRatedDown = true;
      return;
    }

    this.increase(comment, comments);
    comment.isRatedDown = false;
  }

  decrease(comment: any, comments: CommentModel[] | undefined) {
    this.cs.decreaseRating(comment.id)
      .subscribe(() => {
        // @ts-ignore
        let index = comments.findIndex(c => c.id == comment.id);
        // @ts-ignore
        comments[index].rating--;
      });
  }
}
