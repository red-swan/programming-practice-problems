#lang racket


(define (sign x)
  (cond
    [(< x 0) 'negative]
    [(< 0 x) 'positive]
    [else    'zero]))
                
(define (cumdiff xs)
  (let* ([n (length xs)]
         [k (- n 1)])
    (map - (rest xs) (take xs k))))

(define (between a b x)
  (and
   (<= a x)
   (<= x b)))

(define (safe-diff? x dir)
  (match dir
    ['increasing (between 1 3 x)]
    ['decreasing (between 1 3 x)]
    [-1          (safe-diff? x 'decreasing)]
    [ 1          (safe-diff? x 'increasing)]
    [else        #f]))


(define (safe-report? report [tolerance 0])
  (let ([diffs (cumdiff report)])
    (let-values ([(errors dir)]
                 (for/fold ([errors 0]
                            [dir #f])
                           ([d diffs])
                   (if dir
                       (if (safe-diff? d dir)
                           (values errors dir)
                           (values (add1 errors) dir))
                       (values errors (sign d)))))
      (<= errors tolerance))))
             
    

; Read Data

(define reports
  (with-input-from-file "02-sample.txt"
    (λ ()
      (for/list ([line (in-lines)])
        (line->report line)))))

(define (line->report line)
  (map string->number
       (string-split line " ")))


; Part 1


; Part 2


; Scratch

(define asd (map cumdiff reports))
(map count-sign-switches asd)






























 