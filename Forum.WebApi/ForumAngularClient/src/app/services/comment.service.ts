import {HttpClient} from "@angular/common/http";
import {CreateCommentModel} from "../models/comment/create-comment.model";
import {UpdateCommentModel} from "../models/comment/update-comment.model";
import {Injectable} from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class CommentService {
  private baseUrl = 'https://localhost:5001/api/comments';

  constructor(private http: HttpClient) {
  }

  add(model: CreateCommentModel) {
    return this.http.post(this.baseUrl, model);
  }

  update(model: UpdateCommentModel) {
    return this.http.put(this.baseUrl, model);
  }

  delete(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  reply(id: number, model: CreateCommentModel) {
    return this.http.post(`${this.baseUrl}/${id}/reply`, model);
  }

  increaseRating(id: number) {
    return this.http.put(`${this.baseUrl}/${id}/increase-rating`, null);
  }

  decreaseRating(id: number) {
    return this.http.put(`${this.baseUrl}/${id}/decrease-rating`, null);
  }

}
