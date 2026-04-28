import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Member } from '../../types/member';

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  private baseUrl=environment.apiUrl;
  private http=inject(HttpClient);
  likeIds=signal<string[]>([]);

  toggleLike(targetMemberId:string){
    return this.http.post(`${this.baseUrl}likes/${targetMemberId}`,{}).subscribe({
      next:()=>{
        if(this.likeIds().includes(targetMemberId)){
          this.likeIds.update(ids=>ids.filter(x=> x !== targetMemberId))
        }else{
          this.likeIds.update(ids=>[...ids,targetMemberId])
        }
      }
    })
  }
  getLikes(predicate:string){
  return this.http.get<Member[]>(this.baseUrl + 'likes?predicate='+ predicate);
  }
  getLikesIds(){
    return this.http.get<string[]>(this.baseUrl + 'likes/list').subscribe({
      next:ids=>this.likeIds.set(ids)
    })
  }
  clearLikeIds(){
    this.likeIds.set([]);
  }
}

