import { Injectable } from '@angular/core';
import {Router, RoutesRecognized} from '@angular/router';
import {filter, pairwise} from "rxjs";

/** A router wrapper, adding extra functions. */
@Injectable({
  providedIn: 'root'
})
export class RouterExtService {

  private previousUrl: string = '';

  constructor(public router : Router) {
    this.router.events
      .pipe(filter((evt: any) => evt instanceof RoutesRecognized), pairwise())
      .subscribe((events: RoutesRecognized[]) => {
        this.previousUrl = events[0].urlAfterRedirects;
      });
  }

  public getPreviousUrl(){
    return this.previousUrl;
  }
}
