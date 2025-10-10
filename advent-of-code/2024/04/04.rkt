#lang racket

(require racket/hash)

;   x ->->->
; y 0,0  1,0
;
; | 0,1  1,1
; v
; | 0,2  1,2
; v

(define DEV #t)
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

; Read data -------------------------------------------------

(define word-search (build-search-mapping (input-path)))
    
; Part 1 ----------------------------------------------------

(count-occurrences word-search "XMAS")



; Scratch ---------------------------------------------------

(define (add pt1 pt2)
  (match-let ([(list x1 y1) pt1]
              [(list x2 y2) pt2])
    (list (+ x1 x2) (+ y1 y2))))

(define (hash-update-all ht updater)
  (for/hash ([k (hash-keys ht)])
    (values k (updater (hash-ref ht k)))))

(define (pattern-at? word-search at pattern)
  (let* ([n (hash-count pattern)]
         [pattern* (hash-update-all pattern (curry add at))]
         [found (hash-intersect word-search pattern*)])
    (= n (hash-count found))))
    








; 5 0

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



(define (count/word-search word-search patterns)
  (for/fold ([occurrences 0])
             ([pos (hash-keys word-search)])
    (+ occurrences
       (count identity (map (curry pattern-at? word-search pos) patterns)))))
             

             
(count/word-search word-search (build-word-patterns "XMAS"))




















