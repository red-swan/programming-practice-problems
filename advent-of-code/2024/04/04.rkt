#lang racket

(require racket/hash)

;   x ->->->
; y 0,0  1,0
;
; | 0,1  1,1
; v
; | 0,2  1,2
; v

(define DEV #f)
(define input-path (make-parameter "04.txt"))
(when DEV (input-path "04-sample.txt"))

; build a mapping from coordinate to letter
(define (build-search-mapping path)
  (for/fold ([search (hash)]
             [y 0]
             #:result search)
            ([line (file->lines path)])
    (for/fold ([search search]
               [x 0]
               #:result (values search (add1 y)))
              ([char (string->list line)])
      (values
       (hash-set search (list x y) char)
       (add1 x)))))

; Build directions and move from one to another
(define directions '(N S E W NE NW SW SE))
(define (step from dir)
  (match-let ([(list x y) from])
    (match dir
      ['N  (list       x  (sub1 y))]
      ['S  (list       x  (add1 y))]
      ['E  (list (add1 x)       y)]
      ['W  (list (sub1 x)       y)]
      ['NW (list (sub1 x) (sub1 y))]
      ['NE (list (add1 x) (sub1 y))]
      ['SW (list (sub1 x) (add1 y))]
      ['SE (list (add1 x) (add1 y))])))

#|
; Search for word at a location in a direction
(define (has-word? word-search at word dir)
  (cond
    [(equal? word "") #t]
    [(equal? (hash-ref word-search at #f) (string-ref word 0))
     (has-word? word-search (step at dir) (substring word 1) dir)]
    [else #f]))

; Count all occurrences of a word at a location in all directions
(define (count-occurrences-at word-search index word [dir #f])
  (count identity (map (curry has-word? word-search index word) directions)))

; Count all occurrences of the word in a word search
(define (count-occurrences word-search word)
  (for/fold ([count 0])
            ([index (hash-keys word-search)])
    (+ count (count-occurrences-at word-search index word))))
|#
; Read data -------------------------------------------------

(define word-search (build-search-mapping (input-path)))
    
; Part 1 ----------------------------------------------------

;(count-occurrences word-search "XMAS")



; Scratch ---------------------------------------------------


; Add x y coordinates stored as lists

(define (add pt1 pt2)
  (match-let ([(list x1 y1) pt1]
              [(list x2 y2) pt2])
    (list (+ x1 x2) (+ y1 y2))))

; Hash functions

(define (hash-update-keys ht updater)
  (for/hash ([k (hash-keys ht)])
    (values (updater k) (hash-ref ht k))))

(define (hash-intersection ht1 ht2)
  (for/hash ([key1 (hash-keys ht1)]
             #:when (and (hash-has-key? ht2 key1)
                     (equal? (hash-ref ht1 key1)
                             (hash-ref ht2 key1))))
    (values key1 (hash-ref ht1 key1))))

(define (hash-subset? ht ht2)
  (for/fold ([still-subset? #t])
            ([key2 (hash-keys ht2)])
    (if still-subset?
        (and (hash-has-key? ht key2)
             (equal? (hash-ref ht key2)
                     (hash-ref ht2 key2)))
        still-subset?)))
            

; Building patterns for words in a direction

(define (build-word-pattern dir word)
  (for/fold ([at (list 0 0)]
             [indices (hash)]
             #:result indices)
            ([char (string->list word)])
    (values
     (step at dir)
     (hash-set indices at char))))

(define (build-word-patterns word)
  (map (curryr build-word-pattern word) directions))




; Searching for patterns in a word search

(define (pattern-at? word-search at pattern)
  (let* ([n (hash-count pattern)]
         [pattern* (hash-update-keys pattern (curry add at))]
         [found (hash-intersection word-search pattern*)])
    (= n (hash-count found))))
    
(define (find-occurrences word-search pattern)
  (for/fold ([occurrences (set)])
            ([at (hash-keys word-search)])
    (let ([pattern* (hash-update-keys pattern (curry add at))])
      
    (if (hash-subset? word-search pattern*)
        (set-add occurrences pattern*)
        occurrences))))
        
(define (count-occurrences word-search patterns)
  (set-count
   (foldl set-union
          (set)
          (map (curry find-occurrences word-search) patterns))))



(define xmas-patterns (build-word-patterns "XMAS"))
(count-occurrences word-search xmas-patterns)




















