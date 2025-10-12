#lang racket



; Environment setup -------------------------------------------------------------
(require racket/hash)

(define DEV #f)
(define input-path (make-parameter "04.txt"))
(when DEV (input-path "04-sample.txt"))


; Hash Functions ----------------------------------------------------------------

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

(define (hash-has-same-key-value? ht1 ht2 key)
  (and (hash-has-key? ht1)
       (hash-has-key? ht2)
       (equal? (hash-ref ht1 key)
               (hash-ref ht2 key))))

(define (hash-difference ht ht2)
  (hash-filter ht
               (λ (k v) (not
                         (and
                          (hash-has-key? ht2 k)
                          (equal? v (hash-ref ht2 k)))))))

; Coordinate Functions ----------------------------------------------------------

;Add x y coordinates stored as lists
(define (add pt1 pt2)
  (match-let ([(list x1 y1) pt1]
              [(list x2 y2) pt2])
    (list (+ x1 x2) (+ y1 y2))))


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


; Word Search Functions ---------------------------------------------------------
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

; Build patterns for words in a direction
(define (build-word-pattern dir word [start (list 0 0)])
  (for/fold ([at start]
             [indices (hash)]
             #:result indices)
            ([char (string->list word)])
    (values
     (step at dir)
     (hash-set indices at char))))

(define (build-word-patterns word)
  (map (curryr build-word-pattern word) directions))

; Build patterns for cross-mass
(define (build-cross-pattern word a-dir a-coord b-dir b-coord)
  (let ([a (build-word-pattern a-dir word a-coord)]
        [b (build-word-pattern b-dir word b-coord)])
    (hash-union a b #:combine (λ (a b) a))))

; Searching for patterns in a word search
(define (pattern-at? word-search at pattern)
  (let* ([n (hash-count pattern)]
         [pattern* (hash-update-keys pattern (curry add at))]
         [found (hash-intersection word-search pattern*)])
    (= n (hash-count found))))

; 
; returns the word search without the matching patterns
(define (count-occurrences word-search pattern [with-patterns? #f])
  (for/fold ([occurrences 0]
             [found (set)]
             #:result (if with-patterns? (values occurrences found) occurrences))
            ([coordinate (hash-keys word-search)])
    (let ([pattern* (hash-update-keys pattern (curry add coordinate))])
      
      (if (hash-subset? word-search pattern*)
          (values (add1 occurrences)
                  (set-add found pattern*))
          (values occurrences found)))))

; patterns must not have duplicates
(define (count-occurrences* word-search patterns [return-found? #f])
  (for/fold ([found (set)]
             #:result
             (if return-found? found (set-count found)))
            ([pattern patterns])
    (let-values ([(occurrences new-found) (count-occurrences word-search pattern #t)])
      (set-union found new-found))))


; Computing Solutions ---------------------------------------
; Read data -----------------------------

(define word-search (build-search-mapping (input-path)))

; Part 1 --------------------------------
(define xmas-patterns (build-word-patterns "XMAS"))
;(count-occurrences* word-search xmas-patterns)

; Part 2 --------------------------------
(define cross-mass-patterns
         (map (λ (l) (apply (curry build-cross-pattern "MAS") l))
         '(
           (S  (0 0) E  (-1  1))
           (S  (0 0) W  ( 1  1))
           (N  (0 0) E  (-1 -1))
           (N  (0 0) W  ( 1 -1))
           (SE (0 0) NE ( 0  2))
           (SE (0 0) SW ( 2  0))
           (NE (0 0) NW ( 2  0))
           (SW (0 0) NW ( 0  2))
           )))

(count-occurrences* word-search cross-mass-patterns)

; 1279 too low
; answer
; 2052 too high
; Scratch ---------------------------------------------------

;(define asd (set->list (count-occurrences* word-search cross-mass-patterns #t)))

;(define sdf (map (curryr hash-filter (λ (k v) (equal? v #\M))) asd))

;(define zxc (eighth cross-mass-patterns))

;(build-word-pattern 'NW "MAS" (list 0 -2))
#|
08 28
28 48
48 68
68 88

10 12
63 83
12 14
51 71

|#







