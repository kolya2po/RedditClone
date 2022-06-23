import {Injectable} from "@angular/core";
import {CommentModel} from "../models/comment/comment.model";
import {CommentService} from "./comment.service";

@Injectable({
  providedIn : 'root'
})
export class CommentHelperService {

  constructor(private cs: CommentService) {
  }

  increaseRating(comment: any, comments: CommentModel[] | undefined) {
    if (!comment.isRatedUp) {
      this.cs.increaseRating(comment.id)
        .subscribe(() => {
          // @ts-ignore
          let index = comments.findIndex(c => c.id == comment.id);
          // @ts-ignore
          comments[index].rating++;
        });
      comment.isRatedUp = true;
      return;
    }

    this.cs.decreaseRating(comment.id)
      .subscribe(() => {
        // @ts-ignore
        let index = comments.findIndex(c => c.id == comment.id);
        // @ts-ignore
        comments[index].rating--;
      });
    comment.isRatedUp = false;
  }

  decreaseRating(comment: any, comments: CommentModel[] | undefined) {
    if (!comment.isRatedDown) {
      this.cs.decreaseRating(comment.id)
        .subscribe(() => {
          // @ts-ignore
          let index = comments.findIndex(c => c.id == comment.id);
          // @ts-ignore
          comments[index].rating--;
        });
      comment.isRatedDown = true;
      return;
    }

    this.cs.increaseRating(comment.id)
      .subscribe(() => {
        // @ts-ignore
        let index = comments.findIndex(c => c.id == comment.id);
        // @ts-ignore
        comments[index].rating++;
      });
    comment.isRatedDown = false;
  }

}
